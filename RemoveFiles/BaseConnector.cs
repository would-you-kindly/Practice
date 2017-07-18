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
            foreach (Guid key in keys)
            {
                // TODO: Сделать удаление всех файлов за раз.
                if (RequestConfirmation(command, key))
                {
                    RemoveFromFS(command, key);
                    RemoveFromDB(command, key);
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
            parameter.DbType = DbType.Guid;
            sqlCommand.Parameters.Add(parameter);
            sqlCommand.ExecuteNonQuery();
        }

        private bool RequestConfirmation(Command command, Guid key)
        {
            if (command.Confirmation)
            {
                Console.WriteLine($"Вы уверены, что хотите удалить файл {GetFileUrlByKey(command, key)}? (Y/N)");
                ConsoleKeyInfo consoleKey = Console.ReadKey();
                Console.WriteLine();
                if (consoleKey.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
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
