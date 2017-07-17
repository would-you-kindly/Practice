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
            sqlCommand.Parameters.Add("FilesTableName", SqlDbType.NVarChar).Value = command.TableName;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        protected override void RemoveFromDB(Command command, object key)
        {
            // Удаляем запись файла из базы данных.
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("DELETE FROM {0} WHERE {1} = @key", command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", SqlDbType.UniqueIdentifier).Value = key;
            sqlCommand.ExecuteNonQuery();
        }

        protected override DbCommand GetSqlCommand(Command command, object key)
        {
            // Составляем запрос для получения Url.
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = @key", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", SqlDbType.UniqueIdentifier).Value = key;

            return sqlCommand;
        }

        protected override List<object> GetForeignKeysFromTable(List<object> keys, string tableName, string columnName)
        {
            // Получаем список внешних ключей на таблицу файлов.
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {0} IS NOT NULL", columnName, tableName);

            List<object> foreignKeys = new List<object>();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    foreignKeys.Add(reader.GetValue(0));
                }
            }

            return foreignKeys;
        }

        protected override List<object> GetFilesPrimaryKeys(Command command)
        {
            // Получаем список первичных ключей таблицы файлов.
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1}", command.PrimaryKeyFieldName, command.TableName);

            List<object> primaryKeys = new List<object>();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    primaryKeys.Add(reader.GetValue(0));
                }
            }

            return primaryKeys;
        }

        protected override string GetFileNameByKey(Command command, object key)
        {
            SqlCommand sqlCommand = (Connection as SqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = @key", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", SqlDbType.UniqueIdentifier).Value = key;
            string result = (string)sqlCommand.ExecuteScalar();

            return result;
        }
    }
}
