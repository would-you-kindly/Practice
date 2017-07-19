using log4net;
using System.Data;
using System.Data.SqlClient;

namespace RemoveFiles
{
    /// <summary>
    /// 
    /// </summary>
    class SqlServerConnector : BaseConnector
    {
        public SqlServerConnector(ILog logger, string connectionString)
            : base(logger)
        {
            Connection = new SqlConnection(connectionString);
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
    }
}
