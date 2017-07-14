using log4net;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace RemoveFiles
{
    class ConnectorFactory
    {
        public static BaseConnector CreateConnector(Command command)
        {
            // NullLogger пишет в log "пустые" значения.
            // Это позволяет сделать проверку на command.Log
            // один раз, а не перед каждой записью в лог.
            ILog logger = new NullLogger();
            if (command.Log == "true")
            {
                logger = Logger.Log;
            }

            switch (command.Dbms)
            {
                case "SQL Server":
                    return new SqlServerConnector(logger, command.ConnectionString);
                    break;
                case "PostgreSQL":
                    return new PostgreSqlConnector(logger, command.ConnectionString);
                    break;
                default:
                    throw new Exception("");
                    break;
            }
        }
    }
}