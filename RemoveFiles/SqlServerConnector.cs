using log4net;
using System.Data;
using System.Data.SqlClient;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, выполняющию роль 
    /// коннектора для СУБД SQL Server (конкретная стратегия).
    /// </summary>
    class SqlServerConnector : BaseConnector
    {
        /// <summary>
        /// Создает новый экземпляр класса SqlServerConnector
        /// и инициализирует соединение к базе данных.
        /// </summary>
        /// <param name="logger">Объект для выполнения логирования действий программы.</param>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public SqlServerConnector(ILog logger, string connectionString)
            : base(logger)
        {
            Connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Выполняет поиск всех таблиц и их внешних ключей,
        /// ссылающихся на таблицу файлов.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения названия таблицы файлов.</param>
        /// <returns>Таблица с названиями таблиц и внешних ключей.</returns>
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
    }
}
