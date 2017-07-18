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

        protected DbConnection _connection;

        public BaseConnector(ILog logger)
        {
            _logger = logger;
        }

        public virtual void Execute(Command command)
        {
            using (_connection)
            {
                _connection.Open();
                DataTable table = FindReferencingTables(command);
                List<Guid> keys = FindHangingFileKeys(command, table);
                RemoveHangingFiles(command, keys);

                Console.WriteLine("Удаление файлов прошло успешно.");
            }
        }

        protected abstract DataTable FindReferencingTables(Command command);

        protected virtual List<Guid> FindHangingFileKeys(Command command, DataTable table)
        {
            string queryString = string.Empty;

            foreach (DataRow foreignKey in table.Rows)
            {
                queryString += $@"SELECT {command.TableName}.{command.PrimaryKeyFieldName}
                FROM {command.TableName}
                WHERE {command.TableName}.{command.PrimaryKeyFieldName} IS NOT NULL 
                AND {command.TableName}.{command.PrimaryKeyFieldName} NOT IN 
                (SELECT {foreignKey["TableName"]}.{foreignKey["ColumnName"]}
                FROM {foreignKey["TableName"]} 
                WHERE {foreignKey["TableName"]}.{foreignKey["ColumnName"]} IS NOT NULL)
                INTERSECT";
            }

            // TODO: Не уверен здесь, возможно нужно перевернуть строку.
            queryString.TrimEnd("INTERSECT".ToCharArray());

            var sqlCommand = _connection.CreateCommand();
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
            bool removeAll = false;
            RemoveFilesOptions option = RemoveFilesOptions.No;

            foreach (Guid key in keys)
            {
                if (!removeAll)
                {
                    option = RequestConfirmation(command, key);
                }

                // TODO: Сделать удаление всех файлов за раз.
                // TODO: Удалить все/Удалить текущий/Нет для всех/Нет для текущего.
                if (removeAll = option == RemoveFilesOptions.All ? true : false || option == RemoveFilesOptions.Yes)
                {
                    Console.WriteLine(key.ToByteArray().ToString());
                    //RemoveFromFS(command, key);
                    //RemoveFromDB(command, key);
                }
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

            // Удаляем .pdf файл.
            if (pdfFile != null && pdfFile.Exists)
            {
                pdfFile.Delete();
                _logger.Info($"Удален .pdf файл: {pdfFile.FullName}");
            }
        }

        protected virtual void RemoveFromDB(Command command, Guid key)
        {
            // Удаляем запись файла из базы данных.
            DbCommand sqlCommand = _connection.CreateCommand();
            sqlCommand.CommandText = $"DELETE FROM {command.TableName} WHERE {command.PrimaryKeyFieldName} = @key";
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
            if (command.Confirmation)
            {
                Console.WriteLine($"Вы уверены, что хотите удалить файл {GetFileUrlByKey(command, key)}? (A/Y/N)");
                ConsoleKeyInfo consoleKey = Console.ReadKey(true);
                Console.WriteLine();

                switch (consoleKey.Key)
                {
                    case ConsoleKey.A:
                        return RemoveFilesOptions.All;
                    case ConsoleKey.Y:
                        return RemoveFilesOptions.Yes;
                    case ConsoleKey.N:
                        return RemoveFilesOptions.No;
                    default:
                        throw new ArgumentException("Неверно выбран варинат удаления.");
                }
            }
            else
            {
                return RemoveFilesOptions.All;
            }
        }

        protected virtual string GetFileUrlByKey(Command command, Guid key)
        {
            // Получаем название файла по ключу (для лога)
            DbCommand sqlCommand = _connection.CreateCommand();
            sqlCommand.CommandText = $"SELECT {command.UrlFieldName} FROM {command.TableName} WHERE {command.PrimaryKeyFieldName} = @key";
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
