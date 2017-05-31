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
    class Command
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
            Confirmation
        }

        private const string defaultConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;";
        private const string defaultTableName = "Files";
        private const string defaultPrimaryKeyFieldName = "PrimaryKey";
        private const string defaultUrlFieldName = "Url";
        private const string defaultPath = @"С:\";
        private const string defaultLog = "true";
        private const string defaultConfirmation = "true";

        private Dictionary<Commands, string> commands;

        /// <summary>
        /// Создает новый экземпляр класса Command 
        /// и присваивает аргументам программы значения по умолчнию.
        /// </summary>
        public Command()
        {
            commands = new Dictionary<Commands, string>()
            {
                { Commands.Help, null },
                { Commands.ConnectionString, defaultConnectionString },
                { Commands.TableName, defaultTableName },
                { Commands.PrimaryKeyFieldName, defaultPrimaryKeyFieldName },
                { Commands.UrlFieldName, defaultUrlFieldName },
                { Commands.Path, defaultPath },
                { Commands.Log, defaultLog },
                { Commands.Confirmation, defaultConfirmation }
            };
        }

        /// <summary>
        /// Задает или возвращает строку подключения к базе данных.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return commands[Commands.ConnectionString];
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
                    Console.WriteLine("При попытке соединения произошла ошибка. Проверьте правильность строки подключения.");
                    Environment.Exit(0);
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
                return commands[Commands.TableName];
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
                return commands[Commands.PrimaryKeyFieldName];
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
                return commands[Commands.UrlFieldName];
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
                return commands[Commands.Path];
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
                    Console.WriteLine("Указанного пути не существует. Проверьте правильность пути к каталогу.");
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Задает или возвращает значение ("true" или "false"), указывающее, 
        /// необходимо ли выполнять логирование действий программы. 
        /// </summary>
        public string Log
        {
            get
            {
                return commands[Commands.Log];
            }
            set
            {
                // Проверяем правильность параметров команды -log.
                if (value == "true" || value == "false")
                {
                    commands[Commands.Log] = value;
                }
                else
                {
                    Console.WriteLine("Аргумент команды -log указан неверно." +
                        "Он может принимать только значение \"true\" или \"false\".");
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Задает или возвращает значение ("true" или "false"), указывающее, 
        /// необходимо ли запрашивать подтвержедние перед удалением. 
        /// </summary>
        public string Confirmation
        {
            get
            {
                return commands[Commands.Confirmation];
            }
            set
            {
                // Проверяем правильность параметров команды -conf.
                if (value == "true" || value == "false")
                {
                    commands[Commands.Confirmation] = value;
                }
                else
                {
                    Console.WriteLine("Аргумент команды -conf указан неверно. " +
                        "Он может принимать только значение \"true\" или \"false\".");
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Выводит полную справку о программе.
        /// </summary>
        public void Help()
        {
            Console.WriteLine("Вывод справки. Порядок указания важен. Названия файлов хранятся в столбце Name или другое имя? Что делать с .pdf фалами, которые уадлись (по требованию), но запси о них сотались, т.к. ссылки из дургих мест на них до сих пор есть.");

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
                "\n\nЗадание названия таблицы файлов.\n\tКлючевое слово:\n\t\t-tn\n\tПараметры:\n\t\tНазвание таблицы файлов в кавычках.\n\tПример:\n\t\t-tn \"Файлы\"\n\tЗначение по умолчанию:\n\t\t\"Files\"" +
                "\n\nЗадание названия поля первичного ключа.\n\tКлючевое слово:\n\t\t-pk\n\tПараметры:\n\t\tНазвание поля первичного ключа в кавычках.\n\tПример:\n\t\t-pk \"Id\"\n\tЗначение по умолчанию:\n\t\t\"PrimaryKey\"" +
                "\n\nЗадание названия поля с относительным путем к файлу.\n\tКлючевое слово:\n\t\t-url\n\tПараметры:\n\t\tОтносительный путь к каталогу с файлами в кавычках.\n\tПример:\n\t\t-url \"Url\"\n\tЗначение по умолчанию:\n\t\t\"Url\"" +
                "\n\nЗадание пути к каталогу с файлами.\n\tКлючевое слово:\n\t\t-path\n\tПараметры:\n\t\tПуть к каталогу с файлами в кавычках.\n\tПример:\n\t\t-path \"C:\\Windows\\Help\"\n\tЗначение по умолчанию:\n\t\t\"С:\\\"" +
                "\n\nВыполнения логирования действий программы.\n\tКлючевое слово:\n\t\t-log\n\tПараметры:\n\t\t- \"true\" - выполнять логирование.\n\t\t- \"false\" - не выполнять логирование.\n\tПример:\n\t\t-log \"false\"\n\tЗначение по умолчанию:\n\t\t\"true\"" +
                "\n\nЗапрос на подтверждение удаления.\n\tКлючевое слово:\n\t\t-conf\n\tПараметры:\n\t\t- \"true\" - запрашивать подтверждение удаления.\n\t\t- \"false\" - не запрашивать подтверждение удаления.\n\tПример:\n\t\t-conf \"false\"\n\tЗначение по умолчанию:\n\t\t\"true\"\n\n\n");

            Environment.Exit(0);
        }
    }
}
