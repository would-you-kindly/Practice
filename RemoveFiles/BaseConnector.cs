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
    // Стратегия
    abstract class BaseConnector
    {
        protected ILog _logger;

        public DbConnection Connection;

        public BaseConnector(ILog logger)
        {
            _logger = logger;
        }

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

        protected abstract DataTable FindReferencingTables(Command command);

        protected virtual List<Guid> FindHangingFileKeys(Command command, DataTable table)
        {
            // TODO: StringBuilder
            // TODO: Можно поменять здесь на union внутренних запросов и сделать из них except из первичных ключей файлов
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

            return keys;
        }

        protected virtual void RemoveHangingFiles(Command command, List<Guid> keys)
        {
            // TODO: Страшный метод
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
                    Console.WriteLine(key.ToString());
                    //RemoveFromFS(command, key);
                    //RemoveFromDB(command, key);
                }
            }

            if (option == RemoveFilesOptions.NoToAll)
            {
                Console.WriteLine("Удаление файлов отменено.");
            }
            else
            {
                Console.WriteLine("Удаление файлов прошло успешно.");
            }
        }

        protected void RemoveFromFS(Command command, Guid key)
        {
            // Удаляем файлы (включая .pdf) из файловой системы.
            string relativePath = GetFileUrlByKey(command, key);

            FileInfo file = new FileInfo(command.Path + relativePath);
            FileInfo pdfFile = new FileInfo(command.Path + relativePath.Remove(relativePath.LastIndexOf(".")) + ".pdf");

            // Удаляем файл.
            if (file != null && file.Exists)
            {
                file.Delete();
                _logger.Info($"Удален файл: {file.FullName}");
            }

            // TODO: try catch сюда и при удалении записей
            // Удаляем .pdf файл.
            if (pdfFile != null && pdfFile.Exists)
            {
                pdfFile.Delete();
                _logger.Info($"Удален .pdf файл: {pdfFile.FullName}");
            }
        }

        // TODO: Все равно пока только guid в качестве первичного ключа
        protected virtual void RemoveFromDB(Command command, Guid key)
        {
            // Удаляем запись файла из базы данных.
            DbCommand sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = $"DELETE FROM \"{command.TableName}\" WHERE \"{command.PrimaryKeyFieldName}\" = @key";
            DbParameter parameter = sqlCommand.CreateParameter();
            parameter.ParameterName = "key";
            parameter.Value = key;
            // TODO: Тут не уверен насчет типа DbType
            parameter.DbType = DbType.Guid;
            sqlCommand.Parameters.Add(parameter);
            sqlCommand.ExecuteNonQuery();

            _logger.Info($"Удалена запись: {GetFileUrlByKey(command, key)}");
        }

        private RemoveFilesOptions RequestConfirmation(Command command, Guid key)
        {
            if ((bool)command.Confirmation)
            {
                Console.WriteLine($"Вы уверены, что хотите удалить файл {GetFileUrlByKey(command, key)}? (A/Y/N/X)");
                ConsoleKeyInfo consoleKey = Console.ReadKey(true);

                switch (consoleKey.Key)
                {
                    // TODO: переспросить поьзователя что он хотел нажать, если не попал
                    // TODO: добавить комментарии ко все методам

                    case ConsoleKey.A:
                        return RemoveFilesOptions.YesToAll;
                    case ConsoleKey.Y:
                        return RemoveFilesOptions.Yes;
                    case ConsoleKey.N:
                        return RemoveFilesOptions.No;
                    case ConsoleKey.X:
                        return RemoveFilesOptions.NoToAll;
                    default:
                        throw new ArgumentException("Неверно выбран вариант удаления.");
                }
            }
            else
            {
                return RemoveFilesOptions.YesToAll;
            }
        }

        protected virtual string GetFileUrlByKey(Command command, Guid key)
        {
            // Получаем название файла по ключу (для лога)
            DbCommand sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = $"SELECT \"{command.UrlFieldName}\" FROM \"{command.TableName}\" WHERE \"{command.PrimaryKeyFieldName}\" = @key";
            DbParameter parameter = sqlCommand.CreateParameter();
            parameter.ParameterName = "key";
            parameter.Value = key;
            // TODO: Тут не уверен насчет типа DbType
            parameter.DbType = DbType.Guid;
            sqlCommand.Parameters.Add(parameter);
            string result = (string)sqlCommand.ExecuteScalar();

            return result;
        }
    }
}
