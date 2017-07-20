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
    /// <summary>
    /// Представляет сущность для создания коннектора к одной из СУБД.
    /// </summary>
    class ConnectorFactory
    {
        /// <summary>
        /// Создает новый экземпляр коннектора к одной из СУБД.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения строки подключения.</param>
        /// <returns>Новый экземпляр коннектора к одной из СУБД.</returns>
        public static BaseConnector CreateConnector(Command command)
        {
            // В зависимости от необходимости выполнения логирования 
            // создается валидный/невалидный экземпляр logger'а.
            ILog logger = new NullLogger();
            if (command.Log)
            {
                Logger.InitLogger();
                logger = Logger.Log;
            }

            switch (command.Dbms)
            {
                case "SQL Server":
                    return new SqlServerConnector(logger, command.ConnectionString);
                case "PostgreSQL":
                    return new PostgreSqlConnector(logger, command.ConnectionString);
                default:
                    throw new ArgumentException("Неверно указана база данных.");
            }
        }
    }
}