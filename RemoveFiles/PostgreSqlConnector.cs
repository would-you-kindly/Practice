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
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
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
                WHERE constraint_type = 'FOREIGN KEY' AND tc.table_name = @FilesTableName;";
            sqlCommand.Parameters.AddWithValue("FilesTableName", command.TableName);
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        protected override void RemoveFromDB(Command command, Guid key)
        {
            // Удаляем запись файла из базы данных.
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("DELETE FROM {0} WHERE {1} = @key", command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", NpgsqlDbType.Uuid).Value = key;
            sqlCommand.ExecuteNonQuery();
        }

        protected override DbCommand GetSqlCommand(Command command, Guid key)
        {
            // Составляем запрос для получения Url.
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = @key", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.Add("key", NpgsqlDbType.Uuid).Value = key;

            return sqlCommand;
        }

        protected override string GetFileNameByKey(Command command, Guid key)
        {
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = @key", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName);
            sqlCommand.Parameters.AddWithValue("key", key);            
            string result = (string)sqlCommand.ExecuteScalar();

            return result;
        }
    }
}
