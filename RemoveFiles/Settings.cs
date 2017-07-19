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
        /// <summary>
        /// Параметр для задания СУБД.
        /// </summary>
        internal const string DbmsCommand = "-dbms";
        /// <summary>
        /// Параметр для задания строки подключения к базе данных.
        /// </summary>
        internal const string ConnectionStringCommand = "-cs";
        /// <summary>
        /// Параметр для задания названия таблицы файлов.
        /// </summary>
        internal const string TableNameCommand = "-tn";
        /// <summary>
        /// Параметр для задания названия поля первичного ключа.
        /// </summary>
        internal const string PrimaryKeyFieldNameCommand = "-pk";
        /// <summary>
        /// Параметр для задания названия поля с относительным путем к файлу.
        /// </summary>
        internal const string UrlFieldNameCommand = "-url";
        /// <summary>
        /// Параметр для задания абсолютного пути к каталогу с файлами.
        /// </summary>
        internal const string PathCommand = "-path";
        /// <summary>
        /// Параметр, запрашивающий подтверждение перед удалением.
        /// </summary>
        internal const string LogCommand = "-log";
        /// <summary>
        /// Параметр, указывающий, необходимо ли выполнять логирование.
        /// </summary>
        internal const string ConfirmationCommand = "-conf";
        /// <summary>
        /// Команда для вызова справки по программе.
        /// </summary>
        internal const string HelpCommand = "-help";
    }
}
