using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    // Контекст
    class FileCleaner
    {
        private BaseConnector Connector { get; set; }
        private Command Command { get; set; }

        public FileCleaner(Command command)
        {
            Command = command;
            Connector = ConnectorFactory.CreateConnector(Command);
        }

        public void Clean()
        {
            Connector.Execute(Command);
        }
    }
}
