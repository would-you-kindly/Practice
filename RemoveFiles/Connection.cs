using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    class Connection
    {
        public IConnector Connector { get; set; }

        public Connection(IConnector connector)
        {
            Connector = connector;
        }

        public void Execute()
        {
            Connector.Execute();
        }
    }
}
