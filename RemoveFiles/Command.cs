using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using System.Data.Common;
using System.Data;
using Npgsql;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет команды программы по удалению файлов и связанные с ними аргументы.
    /// </summary>
    public class Command
    {
        public enum CommandType
        {
            /// <summary>
            /// Задание СУБД.
            /// </summary>
            Dbms,
            /// <summary>
            /// Задание строки подключения к базе данных.
            /// </summary>
            ConnectionString,
            /// <summary>
            /// Задание имени таблицы с файлами.
            /// </summary>
            TableName,
            /// <summary>
            /// Задание имени поля первичного ключа.
            /// </summary>
            PrimaryKeyFieldName,
            /// <summary>
            /// Задание имени поля, которое хранит относительный путь к файлам.
            /// </summary>
            UrlFieldName,
            /// <summary>
            /// Задание абсолютного пути к папке, в которой находятся файлы.
            /// </summary>
            Path,
            /// <summary>
            /// Выполнять ли логирование данного действия.
            /// </summary>
            Log,
            /// <summary>
            /// Запрашивать ли подтверждение перед удалением.
            /// </summary>
            Confirmation,
            /// <summary>
            /// Вывод справки по программе.
            /// </summary>
            Help
        }

        /// <summary>
        /// Представляют значения аргументов команды по умолчанию.
        /// </summary>
        internal const string DefaultDbms = "PostgreSQL";
        internal const string DefaultConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;";
        internal const string DefaultTableName = "Файл";
        internal const string DefaultPrimaryKeyFieldName = "PrimaryKey";
        internal const string DefaultUrlFieldName = "Url";
        internal const string DefaultPath = @"D:\YandexDisk\Third course\Производственная практика\Practice\TestFiles";
        internal const bool DefaultLog = false;
        internal const bool DefaultConfirmation = false;

        /// <summary>
        /// Представляет коллекцию команд и связанных с ними параметров.
        /// </summary>
        private Dictionary<CommandType, object> commands;

        /// <summary>
        /// Создает новый экземпляр класса Command.
        /// </summary>
        public Command()
        {
            commands = new Dictionary<CommandType, object>()
            {
                { CommandType.ConnectionString, null },
                { CommandType.Dbms, null },
                { CommandType.TableName, null },
                { CommandType.PrimaryKeyFieldName, null },
                { CommandType.UrlFieldName, null },
                { CommandType.Path, null },
                { CommandType.Log, DefaultLog },
                { CommandType.Confirmation, DefaultConfirmation }
            };
        }

        /// <summary>
        /// Проверяет на корректность все аргументы команды.
        /// </summary>
        /// <returns>true, если проверка прошла успешно, иначе false.</returns>
        public bool Validate()
        {
            // Если параметры не заданы при запуске, задаем им значение по умолчанию.
            if (Dbms == null)
            {
                Dbms = DefaultDbms;
            }
            if (ConnectionString == null)
            {
                ConnectionString = DefaultConnectionString;
            }
            if (TableName == null)
            {
                TableName = DefaultTableName;
            }
            if (PrimaryKeyFieldName == null)
            {
                PrimaryKeyFieldName = DefaultPrimaryKeyFieldName;
            }
            if (UrlFieldName == null)
            {
                UrlFieldName = DefaultUrlFieldName;
            }
            if (Path == null)
            {
                Path = DefaultPath;
            }

            List<IValidator> validators = new List<IValidator>();
            validators.Add(new DbmsValidator());
            validators.Add(new ConnectionStringValidator());
            validators.Add(new TableNameValidator());
            validators.Add(new PrimaryKeyFieldValidator());
            validators.Add(new UrlFieldNameValidator());
            validators.Add(new PathValidator());

            // Проверяем параметры на корректные значения.
            foreach (IValidator validator in validators)
            {
                if (!validator.Validate(this))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Задает или возвращает название СУБД.
        /// </summary>
        public string Dbms
        {
            get
            {
                return (string)commands[CommandType.Dbms];
            }
            set
            {
                commands[CommandType.Dbms] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает строку подключения к базе данных.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return (string)commands[CommandType.ConnectionString];
            }
            set
            {
                commands[CommandType.ConnectionString] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает название таблицы файлов базы данных.
        /// </summary>
        public string TableName
        {
            get
            {
                return (string)commands[CommandType.TableName];
            }
            set
            {
                commands[CommandType.TableName] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает название поля в таблице файлов базы данных, которое является первичным ключом.
        /// </summary>
        public string PrimaryKeyFieldName
        {
            get
            {
                return (string)commands[CommandType.PrimaryKeyFieldName];
            }
            set
            {
                commands[CommandType.PrimaryKeyFieldName] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает название поля в таблице файлов базы данных, отвечающее за хранение относительного пути хранения файла.
        /// </summary>
        public string UrlFieldName
        {
            get
            {
                return (string)commands[CommandType.UrlFieldName];
            }
            set
            {
                commands[CommandType.UrlFieldName] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает значение, указывающее, по какому пути необходимо удалять файлы.
        /// </summary>
        public string Path
        {
            get
            {
                return (string)commands[CommandType.Path];
            }
            set
            {
                commands[CommandType.Path] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает значение, указывающее, необходимо ли выполнять логирование действий программы. 
        /// </summary>
        public bool Log
        {
            get
            {
                return (bool)commands[CommandType.Log];
            }
            set
            {
                commands[CommandType.Log] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает значение, указывающее, необходимо ли запрашивать подтвержедние перед удалением. 
        /// </summary>
        public bool Confirmation
        {
            get
            {
                return (bool)commands[CommandType.Confirmation];
            }
            set
            {
                commands[CommandType.Confirmation] = value;
            }
        }

        /// <summary>
        /// Выводит полную справку по программе.
        /// </summary>
        public void Help()
        {
            Console.WriteLine("\nУтилита предназначена для удаления файлов и записей о них из базы данных, " +
                "на которые нет ссылок из других таблиц базы данных. При удалении файлов также выполняется " +
                "удаление файла с расширением .pdf с тем же именем. Утилита позволяет:\n" +
                "\n1. указать СУБД, в которой необходимо выполнить удаление;" +
                "\n2. указать строку подключения к базе данных;" +
                "\n3. указать название таблицы, которая предназначена для хранения информации о файлах;" +
                "\n4. указать название поля, которое является первичным ключом таблицы файлов;" +
                "\n5. указать название поля, которое предназначено для хранения относительного пути к файлу;" +
                "\n6. указать абсолютный путь к каталогу, в котором хранятся файлы;" +
                "\n7. выполнять логирование действий программы." +
                "\n\nДля задания аргументов используются ключевые слова и параметры. " +
                "\n\nВызов справки.\n\tКлючевое слово:\n\t\t-help\n\tПараметры:\n\t\tНет параметров.\n\tПример:\n\t\t-help" +
                "\n\nЗадание строки подключения.\n\tКлючевое слово:\n\t\t-cs\n\tПараметры:\n\t\tСтрока подключения к базе данных в кавычках.\n\tПример:\n\t\t-cs \"Data Source=.\\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;\"\n\tЗначение по умолчанию:\n\t\t\"Data Source=.\\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;\"" +
                "\n\nЗадание СУБД.\n\tКлючевое слово:\n\t\t-dbms\n\tПараметры:\n\t\t- \"SQL Server\"\n\t\t- \"PostgreSQL\"\n\tПример:\n\t\t-dbms \"SQL Server\"\n\tЗначение по умолчанию:\n\t\t\"PostgreSQL\"" +
                "\n\nЗадание названия таблицы файлов.\n\tКлючевое слово:\n\t\t-tn\n\tПараметры:\n\t\tНазвание таблицы файлов в кавычках.\n\tПример:\n\t\t-tn \"Файлы\"\n\tЗначение по умолчанию:\n\t\t\"Файл\"" +
                "\n\nЗадание названия поля первичного ключа.\n\tКлючевое слово:\n\t\t-pk\n\tПараметры:\n\t\tНазвание поля первичного ключа в кавычках.\n\tПример:\n\t\t-pk \"Id\"\n\tЗначение по умолчанию:\n\t\t\"PrimaryKey\"" +
                "\n\nЗадание названия поля с относительным путем к файлу.\n\tКлючевое слово:\n\t\t-url\n\tПараметры:\n\t\tОтносительный путь к каталогу с файлами в кавычках.\n\tПример:\n\t\t-url \"Url\"\n\tЗначение по умолчанию:\n\t\t\"Url\"" +
                "\n\nЗадание пути к каталогу с файлами.\n\tКлючевое слово:\n\t\t-path\n\tПараметры:\n\t\tПуть к каталогу с файлами в кавычках.\n\tПример:\n\t\t-path \"C:\\Windows\\Help\"\n\tЗначение по умолчанию:\n\t\t\"D:\\YandexDisk\\Third course\\Производственная практика\\Practice\\TestFiles\"" +
                "\n\nВыполнения логирования действий программы.\n\tКлючевое слово:\n\t\t-log\n\tПример:\n\t\t-log\n\tЗначение по умолчанию:\n\t\t\"не установлено\"" +
                "\n\nЗапрос на подтверждение удаления.\n\tКлючевое слово:\n\t\t-conf\n\tПример:\n\t\t-conf\n\tЗначение по умолчанию:\n\t\t\"не установлено\"" +
                "\n\nЕсли установлен параметр, запрашивающий подтверждение об удалении, то перед удалением каждого файла будет появляться сообщение с просьбой нажатия соответствующей клавиши:\n\tA - удалить все файлы;\n\tY - удалить текущий файл;\n\tN - пропустить удаление текущего файла;\n\tX - пропустить удаление всех файлов.\n\n\n");
        }
    }
}
