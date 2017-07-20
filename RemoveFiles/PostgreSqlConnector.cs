using log4net;
using Npgsql;
using System.Data;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, выполняющию роль коннектора для СУБД PostgreSQL (конкретная стратегия).
    /// </summary>
    class PostgreSqlConnector : BaseConnector
    {
        /// <summary>
        /// Создает новый экземпляр класса PostgreSqlConnector и инициализирует соединение к базе данных.
        /// </summary>
        /// <param name="logger">Объект для выполнения логирования действий программы.</param>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public PostgreSqlConnector(ILog logger, string connectionString)
            : base(logger)
        {
            Connection = new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// Выполняет поиск всех таблиц и их внешних ключей, ссылающихся на таблицу файлов.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения названия таблицы файлов.</param>
        /// <returns>Таблица с названиями таблиц и внешних ключей.</returns>
        protected override DataTable FindReferencingTables(Command command)
        {
            // Получаем DataTable, содержащий названия таблиц и внешних ключей на таблицу файлов.
            NpgsqlCommand sqlCommand = (Connection as NpgsqlConnection).CreateCommand();
            sqlCommand.CommandText =
                @"SELECT
                  R.TABLE_NAME TableName,
                  R.COLUMN_NAME ColumnName
                FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE u
                  INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS FK
                    ON U.CONSTRAINT_CATALOG = FK.UNIQUE_CONSTRAINT_CATALOG
                       AND U.CONSTRAINT_SCHEMA = FK.UNIQUE_CONSTRAINT_SCHEMA
                       AND U.CONSTRAINT_NAME = FK.UNIQUE_CONSTRAINT_NAME
                  INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE R
                    ON R.CONSTRAINT_CATALOG = FK.CONSTRAINT_CATALOG
                       AND R.CONSTRAINT_SCHEMA = FK.CONSTRAINT_SCHEMA
                       AND R.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                WHERE U.TABLE_NAME = @FilesTableName";
            sqlCommand.Parameters.AddWithValue("FilesTableName", command.TableName);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}
