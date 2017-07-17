using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    // Конкретная стратегия
    class SqlServerConnector : BaseConnector
    {
        public SqlServerConnector(ILog logger, string connectionString)
            : base(logger)
        {
            Connection = new SqlConnection(connectionString);
        }

        protected override DbConnection GetConnection()
        {
            return Connection;
        }

        protected override DataTable FindReferencingTables(Command command)
        {
            // Получаем DataTable, содержащий названия таблиц и внешних ключей на таблицу файлов.
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText =
                @"SELECT 
                   OBJECT_NAME (fk.parent_object_id) TableName,
                   COL_NAME (fkc.parent_object_id, fkc.parent_column_id) ColumnName
                FROM 
                   sys.foreign_keys AS fk
                INNER JOIN 
                   sys.foreign_key_columns AS fkc ON fk.OBJECT_ID = fkc.constraint_object_id
                INNER JOIN 
                   sys.tables t ON t.OBJECT_ID = fkc.referenced_object_id
                WHERE 
                   OBJECT_NAME (fk.referenced_object_id) = @FilesTableName";
            sqlCommand.Parameters.AddWithValue("FilesTableName", command.TableName);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        protected override void RemoveFromDB(Command command, Guid key)
        {
            // Удаляем запись файла из базы данных.
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("DELETE FROM {0} WHERE {1} = @key", command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", SqlDbType.UniqueIdentifier).Value = key;
            sqlCommand.ExecuteNonQuery();
        }

        protected override DbCommand GetSqlCommand(Command command, Guid key)
        {
            // Составляем запрос для получения Url.
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = @key", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", SqlDbType.UniqueIdentifier).Value = key;

            return sqlCommand;
        }

        protected override string GetFileNameByKey(Command command, Guid key)
        {
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = @key", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.AddWithValue("key", key);
            string result = (string)sqlCommand.ExecuteScalar();

            return result;
        }
    }
}
