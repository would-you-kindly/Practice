using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    class SqlServerConnection : IConnector
    {
        public SqlConnection Connection { get; set; }

        public SqlServerConnection()
        {
            Connection = new SqlConnection();
        }

        public SqlServerConnection(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        public void Execute()
        {
            Connection.Open();
            // SQL запрос получения записей без ссылок (разный для разных БД)
            // Удаление записей и файлов (разный для удаления записей по SQL, одинаковый для удаления файлов)
            Connection.Close();
        }

        public DbConnection GetConnection()
        {
            return Connection;
        }
    }
}
