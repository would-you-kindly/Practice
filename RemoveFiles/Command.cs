using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет команды программы по удалению файлов.
    /// </summary>
    public class Command
    {
        public enum Commands
        {
            // Вывод справки по программе.
            Help,
            // Задание строки подключения к базе данных.
            ConnectionString,
            // Задание имени таблицы с файлами.
            TableName,
            // Задание имени поля первичного ключа.
            PrimaryKeyFieldName,
            // Задание имени поля, которое хранит относительный путь к файлам.
            UrlFieldName,
            // Задание пути к папке, в которой находятся файлы (абсолютный).
            Path,
            // Выполнять ли логирование данного действия.
            Log,
            // Запрашивать ли подтверждение перед удалением.
            Confirmation,
            // Задание СУБД
            Dbms
        }

        internal const string DefaultConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;";
        internal const string DefaultTableName = "Файл";
        internal const string DefaultPrimaryKeyFieldName = "PrimaryKey";
        internal const string DefaultUrlFieldName = "Url";
        internal const string DefaultPath = @"D:\YandexDisk\Third course\Производственная практика\Practice\TestFiles";
        internal const bool DefaultLog = false;
        internal const bool DefaultConfirmation = false;
        internal const string DefaultDbms = "PostgreSQL";

        private Dictionary<Commands, object> commands;

        /// <summary>
        /// Создает новый экземпляр класса Command 
        /// и присваивает аргументам программы значения по умолчнию.
        /// </summary>
        public Command()
        {
            commands = new Dictionary<Commands, object>()
            {
                { Commands.Help, null },
                { Commands.ConnectionString, DefaultConnectionString },
                { Commands.TableName, DefaultTableName },
                { Commands.PrimaryKeyFieldName, DefaultPrimaryKeyFieldName },
                { Commands.UrlFieldName, DefaultUrlFieldName },
                { Commands.Path, DefaultPath },
                { Commands.Log, DefaultLog },
                { Commands.Confirmation, DefaultConfirmation },
                { Commands.Dbms, DefaultDbms }
            };
        }

        /// <summary>
        /// Задает или возвращает строку подключения к базе данных.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return (string)commands[Commands.ConnectionString];
            }
            set
            {
                try
                {
                    // Проверяем, работает ли соединение.
                    using (SqlConnection connection = new SqlConnection(value))
                    {
                        connection.Open();
                    }

                    commands[Commands.ConnectionString] = value;
                }
                catch (Exception)
                {
                    throw new ArgumentException("При попытке соединения произошла ошибка. Проверьте правильность строки подключения.");
                }
            }
        }

        /// <summary>
        /// Задает или возвращает название таблицы файлов базы данных.
        /// </summary>
        public string TableName
        {
            get
            {
                return (string)commands[Commands.TableName];
            }
            set
            {
                commands[Commands.TableName] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает название поля в таблице файлов 
        /// базы данных, которое является первичным ключом.
        /// </summary>
        public string PrimaryKeyFieldName
        {
            get
            {
                return (string)commands[Commands.PrimaryKeyFieldName];
            }
            set
            {
                commands[Commands.PrimaryKeyFieldName] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает название поля в таблице файлов базы данных,
        /// отвечающее за хранение относительного пути хранения файла.
        /// </summary>
        public string UrlFieldName
        {
            get
            {
                return (string)commands[Commands.UrlFieldName];
            }
            set
            {
                commands[Commands.UrlFieldName] = value;
            }
        }

        /// <summary>
        /// Задает или возвращает значение, указывающее,
        /// по какому пути необходимо удалять файлы.
        /// </summary>
        public string Path
        {
            get
            {
                return (string)commands[Commands.Path];
            }
            set
            {
                // Проверяем, существует ли указанный путь.
                DirectoryInfo directory = new DirectoryInfo(value);
                if (directory.Exists)
                {
                    commands[Commands.Path] = value;
                }
                else
                {
                    throw new ArgumentException("Указанного пути не существует. Проверьте правильность пути к каталогу.");
                }
            }
        }

        /// <summary>
        /// Задает или возвращает значение ("true" или "false"), указывающее, 
        /// необходимо ли выполнять логирование действий программы. 
        /// </summary>
        public bool Log
        {
            get
            {
                return (bool)commands[Commands.Log];
            }
            set
            {
                // Проверяем правильность параметров команды -log.
                if (value != null)
                {
                    commands[Commands.Log] = value;
                }
                else
                {
                    throw new ArgumentException("Аргумент команды -log указан неверно." +
                        "Он может принимать только значение \"true\" или \"false\".");
                }
            }
        }

        /// <summary>
        /// Задает или возвращает значение ("true" или "false"), указывающее, 
        /// необходимо ли запрашивать подтвержедние перед удалением. 
        /// </summary>
        public bool Confirmation
        {
            get
            {
                return (bool)commands[Commands.Confirmation];
            }
            set
            {
                // Проверяем правильность параметров команды -conf.
                if (value != null)
                {
                    commands[Commands.Confirmation] = value;
                }
                else
                {
                    throw new ArgumentException("Аргумент команды -conf указан неверно. " +
                        "Он может принимать только значение \"true\" или \"false\".");
                }
            }
        }

        public string Dbms
        {
            get
            {
                return (string)commands[Commands.Dbms];
            }
            set
            {
                // Проверяем правильность параметров команды -conf.
                if (value == "SQL Server" || value == "PostgreSQL")
                {
                    commands[Commands.Dbms] = value;
                }
                else
                {
                    throw new ArgumentException("Аргумент команды -dbms указан неверно. " +
                        "Он может принимать только значение \"SQL Server\" или \"PostgreSQL\".");
                }
            }
        }

        /// <summary>
        /// Выводит полную справку о программе.
        /// </summary>
        public void Help()
        {
            Console.WriteLine("\nУтилита предназначена для удаления файлов и записей о них из базы данных, " +
                "на которые нет ссылок из других таблиц базы данных. При удалении файлов также выполняется " +
                "удаление файла с расширением .pdf с тем же именем. Утилита позволяет:\n" +
                "\n1. указать строку подключения к базе данных, в которой необходимо выполнить удаление;" +
                "\n2. указать название таблицы, которая предназначена для хранения информации о файлах;" +
                "\n3. указать название поля, которое является первичным ключом таблицы файлов;" +
                "\n4. указать название поля, которое предназначено для хранения относительного пути к файлу;" +
                "\n5. указать абсолютный путь к каталогу, в котором хранятся файлы;" +
                "\n6. выполнять логирование действий программы." +
                "\n\nДля задания аргументов используются ключевые слова и параметры. " +
                "\n\nВызов справки.\n\tКлючевое слово:\n\t\t-help\n\tПараметры:\n\t\tНет параметров.\n\tПример:\n\t\t-help" +
                "\n\nЗадание строки подключения.\n\tКлючевое слово:\n\t\t-cs\n\tПараметры:\n\t\tСтрока подключения к базе данных в кавычках.\n\tПример:\n\t\t-cs \"Data Source=.\\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;\"\n\tЗначение по умолчанию:\n\t\t\"Data Source=.\\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;\"" +
                "\n\nЗадание названия таблицы файлов.\n\tКлючевое слово:\n\t\t-tn\n\tПараметры:\n\t\tНазвание таблицы файлов в кавычках.\n\tПример:\n\t\t-tn \"Файлы\"\n\tЗначение по умолчанию:\n\t\t\"Файл\"" +
                "\n\nЗадание названия поля первичного ключа.\n\tКлючевое слово:\n\t\t-pk\n\tПараметры:\n\t\tНазвание поля первичного ключа в кавычках.\n\tПример:\n\t\t-pk \"Id\"\n\tЗначение по умолчанию:\n\t\t\"PrimaryKey\"" +
                "\n\nЗадание названия поля с относительным путем к файлу.\n\tКлючевое слово:\n\t\t-url\n\tПараметры:\n\t\tОтносительный путь к каталогу с файлами в кавычках.\n\tПример:\n\t\t-url \"Url\"\n\tЗначение по умолчанию:\n\t\t\"Url\"" +
                "\n\nЗадание пути к каталогу с файлами.\n\tКлючевое слово:\n\t\t-path\n\tПараметры:\n\t\tПуть к каталогу с файлами в кавычках.\n\tПример:\n\t\t-path \"C:\\Windows\\Help\"\n\tЗначение по умолчанию:\n\t\t\"D:\\YandexDisk\\Third course\\Производственная практика\\Practice\\TestFiles\"" +
                "\n\nВыполнения логирования действий программы.\n\tКлючевое слово:\n\t\t-log\n\tПараметры:\n\t\t- \"true\" - выполнять логирование.\n\t\t- \"false\" - не выполнять логирование.\n\tПример:\n\t\t-log \"false\"\n\tЗначение по умолчанию:\n\t\t\"true\"" +
                "\n\nЗапрос на подтверждение удаления.\n\tКлючевое слово:\n\t\t-conf\n\tПараметры:\n\t\t- \"true\" - запрашивать подтверждение удаления.\n\t\t- \"false\" - не запрашивать подтверждение удаления.\n\tПример:\n\t\t-conf \"false\"\n\tЗначение по умолчанию:\n\t\t\"true\"" +
                "\n\nЗадание СУБД.\n\tКлючевое слово:\n\t\t-dbms\n\tПараметры:\n\t\t- \"SQL Server\"\n\t\t- \"PostgreSQL\"\n\tПример:\n\t\t-dbms \"SQL Server\"\n\tЗначение по умолчанию:\n\t\t\"PostgreSQL\"\n\n\n");
        }
    }
}
