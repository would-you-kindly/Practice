using log4net;
using Npgsql;
using System.Data;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, выполняющию роль 
    /// коннектора для СУБД PostgreSQL (конкретная стратегия).
    /// </summary>
    class PostgreSqlConnector : BaseConnector
    {
        /// <summary>
        /// Создает новый экземпляр класса PostgreSqlConnector
        /// и инициализирует соединение к базе данных.
        /// </summary>
        /// <param name="logger">Объект для выполнения логирования действий программы.</param>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public PostgreSqlConnector(ILog logger, string connectionString)
            : base(logger)
        {
            Connection = new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// Выполняет поиск всех таблиц и их внешних ключей,
        /// ссылающихся на таблицу файлов.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения названия таблицы файлов.</param>
        /// <returns>Таблица с названиями таблиц и внешних ключей.</returns>
        protected override DataTable FindReferencingTables(Command command)
        {
            // TODO: Скорее всего неправильный запрос.
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
    }
}
