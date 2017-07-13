using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using NpgsqlTypes;

namespace RemoveFiles
{
    class PostgreSqlConnection : IConnector
    {
        public NpgsqlConnection Connection { get; set; }

        public PostgreSqlConnection()
        {
            Connection = new NpgsqlConnection();
        }

        public PostgreSqlConnection(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
        }

        public void Execute()
        {
            Connection.Open();
            //
            Connection.Close();
        }

        public DbConnection GetConnection()
        {
            return Connection;
        }
    }
}
