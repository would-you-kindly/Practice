using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет параметры программы по удалению файлов.
    /// </summary>
    public class Settings
    {
        internal const string HelpCommand = "-help";
        internal const string ConnectionStringCommand = "-cs";
        internal const string TableNameCommand = "-tn";
        internal const string PrimaryKeyFieldNameCommand = "-pk";
        internal const string UrlFieldNameCommand = "-url";
        internal const string PathCommand = "-path";
        internal const string LogCommand = "-log";
        internal const string ConfirmationCommand = "-conf";
        internal const string DbmsCommand = "-dbms";
    }
}
