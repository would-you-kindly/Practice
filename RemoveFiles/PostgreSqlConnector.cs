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
            throw new NotImplementedException();
        }

        protected override void RemoveFromDB(Command command, object key)
        {
            throw new NotImplementedException();
        }
    }
}
