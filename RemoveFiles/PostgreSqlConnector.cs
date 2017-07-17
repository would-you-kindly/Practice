using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using NpgsqlTypes;
using System.IO;
using System.Data.SqlClient;
using log4net;
using System.Data;

namespace RemoveFiles
{
    // Конкретная стратегия
    class PostgreSqlConnector : BaseConnector
    {
        public PostgreSqlConnector(ILog logger, string connectionString)
            : base(logger)
        {
            Connection = new NpgsqlConnection(connectionString);
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
                  ccu.table_name  AS TableName,
                  kcu.column_name AS ColumnName
                FROM
                  information_schema.table_constraints AS tc
                  JOIN information_schema.key_column_usage AS kcu
                    ON tc.constraint_name = kcu.constraint_name
                  JOIN information_schema.constraint_column_usage AS ccu
                    ON ccu.constraint_name = tc.constraint_name
                WHERE constraint_type = 'FOREIGN KEY' AND tc.table_name = 'mspsubject';";
            sqlCommand.Parameters.Add("FilesTableName", SqlDbType.NVarChar).Value = command.TableName;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        protected override void RemoveFromDB(Command command, object key)
        {
            // Удаляем запись файла из базы данных.
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("DELETE FROM {0} WHERE {1} = @key", command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", NpgsqlDbType.Uuid).Value = key;
            sqlCommand.ExecuteNonQuery();
        }

        protected override DbCommand GetSqlCommand(Command command, object key)
        {
            // Составляем запрос для получения Url.
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = @key", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", NpgsqlDbType.Uuid).Value = key;

            return sqlCommand;
        }

        protected override List<object> GetForeignKeysFromTable(List<object> keys, string tableName, string columnName)
        {
            // Получаем список внешних ключей на таблицу файлов.
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {0} IS NOT NULL", columnName, tableName);

            List<object> foreignKeys = new List<object>();
            using (NpgsqlDataReader reader = sqlCommand.ExecuteReader())
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
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1}", command.PrimaryKeyFieldName, command.TableName);

            List<object> primaryKeys = new List<object>();
            using (NpgsqlDataReader reader = sqlCommand.ExecuteReader())
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
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = @key", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", NpgsqlDbType.Uuid).Value = key;
            string result = (string)sqlCommand.ExecuteScalar();

            return result;
        }
    }
}
