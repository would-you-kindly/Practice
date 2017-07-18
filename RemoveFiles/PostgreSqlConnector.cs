using log4net;
using Npgsql;
using System.Data;

namespace RemoveFiles
{
    // Конкретная стратегия
    class PostgreSqlConnector : BaseConnector
    {
        public PostgreSqlConnector(ILog logger, string connectionString)
            : base(logger)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        protected override DataTable FindReferencingTables(Command command)
        {
            // TODO: Скорее всего неправильный запрос.
            // Получаем DataTable, содержащий названия таблиц и внешних ключей на таблицу файлов.
            NpgsqlCommand sqlCommand = (_connection as NpgsqlConnection).CreateCommand();
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
    }
}
