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
                Settings settings = new Settings(args);
                settings.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);               
            }
        }
    }
}
