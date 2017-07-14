using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser parser = new Parser();
                Command command = parser.ParseArgs(args);
                FileCleaner cleaner = new FileCleaner(command);
                cleaner.Clean();
                Settings settings = new Settings(args);
                settings.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);               
            }
        }
    }

    class FileCleaner /*контекст, вместо settings и connection*/
    {
        IConnector cn;

        public FileCleaner(Command cmd)
        {
            cn = new LoggingConnector(ConnectorFactory.CreateConnector(cmd));
        }

        public void Clean()
        {
            cn.Execute();
        }
    }

    abstract class BaseConnector /*стратегия, вместо Iconnector*/
    {
        public virtual void Execute(Command cmd)
        {
            using (DBConnection cn = GetConnection())
            {
                cn.open();
                FindReferencingTables();
                FindHangingFileKeys();
                RemoveHangingFiles();
            }
        }

        protected virtual void RemoveHangingFiles()
        {
            RemoveFromDB();
            RemoveFromFS();
        }

        protected virtual void RemoveFromFS()
        { }
    }

    class LoggingConnector:BaseConnector /*декоратор*/
    {
        BaseConnector loggedConnector;

        public LoggingConnector(BaseConnector connector)
        {
            loggedConnector = connector;
        }

        public override void Execute(Command cmd)
        {
            Logger.Log.Info("ExecuteStart");
            loggedConnector.Execute(cmd);
            Logger.Log.Info("ExecuteFinish");
        }
    }
}



