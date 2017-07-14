using System;
using System.Collections.Generic;
using System.Data.Common;
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
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
