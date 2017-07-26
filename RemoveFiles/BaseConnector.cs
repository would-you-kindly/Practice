using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, выполняющую роль коннектора к СУБД (стратегия).
    /// </summary>
    abstract class BaseConnector
    {
        protected ILog _logger;

        public DbConnection Connection;

        /// <summary>
        /// Создает новый экземпляр класса BaseConnector и инициализирует logger.
        /// </summary>
        /// <param name="logger">Объект для выполнения логирования действий программы.</param>
        public BaseConnector(ILog logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Выполняет удаление файлов из файловой системы и записей из базы данных.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения параметров программы.</param>
        public virtual void Execute(Command command)
        {
            using (Connection)
            {
                Connection.Open();
                DataTable table = FindReferencingTables(command);
                List<Guid> keys = FindHangingFileKeys(command, table);
                RemoveHangingFiles(command, keys);
            }
        }

        /// <summary>
        /// Выполняет поиск всех таблиц и их внешних ключей, ссылающихся на таблицу файлов.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения названия таблицы файлов.</param>
        /// <returns>Таблица с названиями таблиц и внешних ключей.</returns>
        protected abstract DataTable FindReferencingTables(Command command);

        /// <summary>
        /// Выполняет поиск первичных ключей таблицы файлов, на которые нет ссылок из других таблиц.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения параметров программы.</param>
        /// <param name="table">Таблица с названиями таблиц и их внешних ключей на таблицу файлов.</param>
        /// <returns>Список первичных ключей, не имеющих ссылок из других таблиц.</returns>
        protected virtual List<Guid> FindHangingFileKeys(Command command, DataTable table)
        {
            List<string> subqueries = new List<string>();

            foreach (DataRow foreignKey in table.Rows)
            {
                subqueries.Add(string.Format("SELECT \"{0}\".\"{1}\" FROM \"{0}\" WHERE \"{0}\".\"{1}\" IS NOT NULL",
                    foreignKey["TableName"], foreignKey["ColumnName"]));
            }

            string queryString = string.Format("SELECT \"{0}\".\"{1}\" FROM \"{0}\" WHERE \"{0}\".\"{1}\" IS NOT NULL",
                command.TableName, command.PrimaryKeyFieldName) + " EXCEPT (" + string.Join(" UNION ", subqueries) + ")";

            var sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = queryString;

            List<Guid> keys = new List<Guid>();
            using (var reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    keys.Add((Guid)reader.GetValue(0));
                }
            }

            _logger.InfoFormat($"Найдено {keys.Count} файлов, на которые нет ссылок.");
            Console.WriteLine($"Найдено {keys.Count} файлов, на которые нет ссылок.");

            return keys;
        }

        /// <summary>
        /// Выполняет удаление файлов из файловой системы и записей из базы данных.
        /// </summary>
        /// <param name="command">Команда, необходима для получения аргументов программы.</param>
        /// <param name="keys">Список первичных ключей, по которым необходимо выполнить удаление.</param>
        protected virtual void RemoveHangingFiles(Command command, List<Guid> keys)
        {
            bool removeAll = false;
            RemoveFilesOptions option = RemoveFilesOptions.No;

            foreach (Guid key in keys)
            {
                if (!removeAll)
                {
                    option = RequestConfirmation(command, key);
                    if (option == RemoveFilesOptions.NoToAll)
                    {
                        break;
                    }
                    removeAll = option == RemoveFilesOptions.YesToAll;
                }

                if (removeAll || option == RemoveFilesOptions.Yes)
                {
                    RemoveFromFS(command, key);
                    RemoveFromDB(command, key);
                }
            }

            if (option == RemoveFilesOptions.NoToAll)
            {
                _logger.InfoFormat("Удаление файлов отменено.");
                Console.WriteLine("Удаление файлов отменено.");
            }
            else
            {
                _logger.InfoFormat("Удаление файлов прошло успешно.");
                Console.WriteLine("Удаление файлов прошло успешно.");
            }
        }

        /// <summary>
        /// Выполняет удаление файла (включая .pdf) из файловой системы.
        /// </summary>
        /// <param name="command">Команда, необходима для получения абсолютного пути к файлу.</param>
        /// <param name="key">Первичный ключ, по которому необходимо удалить файл.</param>
        protected void RemoveFromFS(Command command, Guid key)
        {
            // Удаляем файлы (включая .pdf) из файловой системы.
            string relativePath = GetFileUrlByKey(command, key);

            FileInfo file = new FileInfo(command.Path + relativePath.TrimStart('~'));
            FileInfo pdfFile = new FileInfo(command.Path + relativePath.TrimStart('~').Remove(relativePath.LastIndexOf(".")) + "pdf");

            // Удаляем файл.
            if (file != null && file.Exists)
            {
                try
                {
                    file.Delete();
                    _logger.Info($"Удален файл: {file.FullName}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    _logger.Error("Удаление файла отменено: " + e.Message);
                }
            }

            // Удаляем .pdf файл.
            if (pdfFile != null && pdfFile.Exists)
            {
                try
                {
                    pdfFile.Delete();
                    _logger.Info($"Удален .pdf файл: {pdfFile.FullName}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    _logger.Error("Удаление .pdf файла отменено: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Выполняет удаление записи из базы данных.
        /// </summary>
        /// <param name="command">Команда, необходима для получения аргументов программы.</param>
        /// <param name="key">Первичный ключ, по которому необходимо удалить запись.</param>
        protected virtual void RemoveFromDB(Command command, Guid key)
        {
            // Удаляем запись файла из базы данных.
            DbCommand sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = $"DELETE FROM \"{command.TableName}\" WHERE \"{command.PrimaryKeyFieldName}\" = @key";
            DbParameter parameter = sqlCommand.CreateParameter();
            parameter.ParameterName = "key";
            parameter.Value = key;
            parameter.DbType = DbType.Guid;
            sqlCommand.Parameters.Add(parameter);

            try
            {
                string record = GetFileUrlByKey(command, key);
                sqlCommand.ExecuteNonQuery();
                _logger.Info($"Удалена запись: {record}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _logger.Error("Удаление записи отменено: " + e.Message);
            }
        }

        /// <summary>
        /// Запрашивает подтверждение на удаление.
        /// </summary>
        /// <param name="command">Команда, необходима для получения аргументов программы.</param>
        /// <param name="key">Первичный ключ, по которому необходимо выполнить удаление.</param>
        /// <returns>Вариант удаления.</returns>
        private RemoveFilesOptions RequestConfirmation(Command command, Guid key)
        {
            if (command.Confirmation)
            {
                Console.WriteLine($"Вы уверены, что хотите удалить файл {GetFileUrlByKey(command, key)}? (A/Y/N/X)");

                while (true)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.A:
                            return RemoveFilesOptions.YesToAll;
                        case ConsoleKey.Y:
                            return RemoveFilesOptions.Yes;
                        case ConsoleKey.N:
                            return RemoveFilesOptions.No;
                        case ConsoleKey.X:
                            return RemoveFilesOptions.NoToAll;
                        default:
                            Console.WriteLine("Нажата неверная клавиша. Можно нажимать только клавиши A/Y/N/X. Проверьте раскладку клавиатуры.");
                            break;
                    }
                }
            }
            else
            {
                return RemoveFilesOptions.YesToAll;
            }
        }

        /// <summary>
        /// Получает относительный путь к файлу.
        /// </summary>
        /// <param name="command">Команда, необходима для получения аргументов программы.</param>
        /// <param name="key">Первичный ключ, по которому находится относитлеьный путь к файлу.</param>
        /// <returns>Строка относительного пути к файлу.</returns>
        protected virtual string GetFileUrlByKey(Command command, Guid key)
        {
            // Получаем название файла по ключу.
            DbCommand sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = $"SELECT \"{command.UrlFieldName}\" FROM \"{command.TableName}\" WHERE \"{command.PrimaryKeyFieldName}\" = @key";
            DbParameter parameter = sqlCommand.CreateParameter();
            parameter.ParameterName = "key";
            parameter.Value = key;
            parameter.DbType = DbType.Guid;
            sqlCommand.Parameters.Add(parameter);
            string result = (string)sqlCommand.ExecuteScalar();

            return result;
        }
    }
}
