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

        public BaseConnector(ILog logger)
        {
            _logger = logger;
        }

        protected DbConnection Connection { get; set; }

        public virtual void Execute(Command command)
        {
            using (Connection = GetConnection())
            {
                Connection.Open();
                DataTable table = FindReferencingTables(command);
                List<Guid> keys = FindHangingFileKeys(command, table);
                RemoveHangingFiles(command, keys);

                Console.WriteLine("Удаление файлов прошло успешно.");
            }
        }

        protected abstract DbConnection GetConnection();

        protected abstract DataTable FindReferencingTables(Command command);

        private List<Guid> FindHangingFileKeys(Command command, DataTable table)
        {
            // Получаем список ключей файлов, на которые есть ссылки из других таблиц.
            List<Guid> keysWithRefs = new List<Guid>();
            foreach (DataRow row in table.Rows)
            {
                // Получаем список внешних ключей из указанной таблицы.
                List<Guid> foreignKeys = GetForeignKeysFromTable(keysWithRefs, (string)row["TableName"], (string)row["ColumnName"]);

                // Собираем внешние ключи в список так, чтобы они не повторялись.
                foreach (Guid foreignKey in foreignKeys)
                {
                    if (!keysWithRefs.Contains(foreignKey))
                    {
                        keysWithRefs.Add(foreignKey);
                    }
                }
            }

            // Получаем список первичных ключей таблицы файлов.
            List<Guid> primaryKeys = GetFilesPrimaryKeys(command);

            // Определяем записи, на которые нет ссылок и удаляем их.
            foreach (Guid primaryKey in primaryKeys)
            {
                if (keysWithRefs.Contains(primaryKey))
                {
                    primaryKeys.Remove(primaryKey);
                }
            }

            return primaryKeys;
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
            DbCommand sqlCommand = GetSqlCommand(command, key);
            string relativePath = (string)sqlCommand.ExecuteScalar();

            FileInfo file = new FileInfo(command.Path + relativePath);
            FileInfo pdfFile = new FileInfo(command.Path + relativePath.Remove(relativePath.LastIndexOf(".")) + ".pdf");

            // Удаляем файл.
            if (file != null && file.Exists)
            {
                file.Delete();
                _logger.InfoFormat("Удален файл: {0}", file.FullName);
            }

            // Удаляем .pdf файл.
            if (pdfFile != null && pdfFile.Exists)
            {
                pdfFile.Delete();
                _logger.InfoFormat("Удален .pdf файл: {0}", pdfFile.FullName);
            }
        }

        protected abstract void RemoveFromDB(Command command, Guid key);

        protected abstract DbCommand GetSqlCommand(Command command, Guid key);

        protected virtual List<Guid> GetForeignKeysFromTable(List<Guid> keys, string tableName, string columnName)
        {
            // Получаем список внешних ключей на таблицу файлов.
            var sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {0} IS NOT NULL", columnName, tableName);

            List<Guid> foreignKeys = new List<Guid>();
            using (var reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    foreignKeys.Add((Guid)reader.GetValue(0));
                }
            }

            return foreignKeys;
        }

        protected virtual List<Guid> GetFilesPrimaryKeys(Command command)
        { 
            // Получаем список первичных ключей таблицы файлов.
            var sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1}", command.PrimaryKeyFieldName, command.TableName);

            List<Guid> primaryKeys = new List<Guid>();
            using (var reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    primaryKeys.Add((Guid)reader.GetValue(0));
                }
            }

            return primaryKeys;
        }

        private bool RequestConfirmation(Command command, Guid key)
        {
            if (command.Confirmation)
            {
                Console.WriteLine(string.Format("Вы уверены, что хотите удалить файл {0}? (Y/N)", GetFileNameByKey(command, key)));
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

        protected abstract string GetFileNameByKey(Command command, Guid key);
    }
}
