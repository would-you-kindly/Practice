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
                List<object> keys = FindHangingFileKeys(command, table);
                RemoveHangingFiles(command, keys);
            }
        }

        protected abstract DbConnection GetConnection();

        protected abstract DataTable FindReferencingTables(Command command);

        private List<object> FindHangingFileKeys(Command command, DataTable table)
        {
            // Получаем список ключей файлов, на которые есть ссылки из других таблиц.
            List<object> keysWithRefs = new List<object>();
            foreach (DataRow row in table.Rows)
            {
                // Получаем список внешних ключей из указанной таблицы.
                List<object> foreignKeys = GetForeignKeysFromTable(keysWithRefs, (string)row["TableName"], (string)row["ColumnName"]);

                // Собираем внешние ключи в список так, чтобы они не повторялись.
                foreach (object foreignKey in foreignKeys)
                {
                    if (!keysWithRefs.Contains(foreignKey))
                    {
                        keysWithRefs.Add(foreignKey);
                    }
                }
            }

            // Получаем список первичных ключей таблицы файлов.
            List<object> primaryKeys = GetFilesPrimaryKeys(command);

            // Определяем записи, на которые нет ссылок и удаляем их.
            foreach (object primaryKey in primaryKeys)
            {
                if (keysWithRefs.Contains(primaryKey))
                {
                    primaryKeys.Remove(primaryKey);
                }
            }

            return primaryKeys;
        }

        protected virtual void RemoveHangingFiles(Command command, List<object> keys)
        {
            foreach (var key in keys)
            {
                RemoveFromFS(command, key);
                RemoveFromDB(command, key);
            }
        }

        protected void RemoveFromFS(Command command, object key)
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

        protected abstract void RemoveFromDB(Command command, object key);

        protected abstract DbCommand GetSqlCommand(Command command, object key);

        protected abstract List<object> GetForeignKeysFromTable(List<object> keys, string tableName, string columnName);

        protected abstract List<object> GetFilesPrimaryKeys(Command command);
    }
}
