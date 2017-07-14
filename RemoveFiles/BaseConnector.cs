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
                List<object> keys = FindHangingFileKeys(command);
                RemoveHangingFiles(command, keys);
            }
        }

        protected abstract DbConnection GetConnection();

        protected abstract DataTable FindReferencingTables(Command command);

        private List<object> FindHangingFileKeys(Command command)
        {
            return new List<object>(); // keys
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
            DbCommand sqlCommand = Connection.CreateCommand();
            /////*Это можно винести в другую абстрактную функцию*/
            // здесь лучше сделать параметризованный запрос (чтобы кавычки, кв. скобки... сами вставлялись)
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName, key);
            string relativePath = (string)sqlCommand.ExecuteScalar();
            /*Это можно винести в другую абстрактную функцию*/////

            FileInfo file = new FileInfo(command.Path + relativePath);
            FileInfo pdfFile = new FileInfo(command.Path + relativePath.Remove(relativePath.LastIndexOf(".")) + ".pdf");

            // Удаляем файл.
            if (file != null && file.Exists)
            {
                file.Delete();
            }

            // Удаляем .pdf файл.
            if (pdfFile != null && pdfFile.Exists)
            {
                pdfFile.Delete();
            }
        }

        protected abstract void RemoveFromDB(Command command, object key);
    }
}
