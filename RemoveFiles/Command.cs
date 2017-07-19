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
    /// Представляет команды программы по удалению файлов.
    /// </summary>
    public class Command
    {
        public enum CommandType
        {
            // Вывод справки по программе.
            Help,
            // Задание строки подключения к базе данных.
            ConnectionString,
            // Задание СУБД
            Dbms,
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

        internal const string DefaultConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;";
        internal const string DefaultDbms = "PostgreSQL";
        internal const string DefaultTableName = "Файл";
        internal const string DefaultPrimaryKeyFieldName = "PrimaryKey";
        internal const string DefaultUrlFieldName = "Url";
        internal const string DefaultPath = @"D:\YandexDisk\Third course\Производственная практика\Practice\TestFiles";
        internal const bool DefaultLog = false;
        internal const bool DefaultConfirmation = false;

        private Dictionary<CommandType, object> commands;

        /// <summary>
        /// Создает новый экземпляр класса Command 
        /// и присваивает аргументам программы значения по умолчнию.
        /// </summary>
        public Command()
        {
            commands = new Dictionary<CommandType, object>()
            {
                // TODO: Здесь может быть ошибка, если знаения по умолчанию не проверяются (например, если не задать строку подлкючения, то возмется строка по умолчанию, и она не проверится)
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

        public bool Validate()
        {
            // Если параметры не заданы при запуске, задаем им значение по умолчанию.
            if (Dbms == null)
                Dbms = DefaultDbms;
            if (ConnectionString == null)
                ConnectionString = DefaultConnectionString;
            if (TableName == null)
                TableName = DefaultTableName;
            if (PrimaryKeyFieldName == null)
                PrimaryKeyFieldName = DefaultPrimaryKeyFieldName;
            if (UrlFieldName == null)
                UrlFieldName = DefaultUrlFieldName;
            if (Path == null)
                Path = DefaultPath;

            // Проверяем все параметры на корректные значения.
            if (ValidateDbms() & ValidateConnectionString() & ValidateTableName() &
               ValidatePrimaryKeyFieldName() & ValidateUrlFieldName() & ValidatePath())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateDbms()
        {
            switch (Dbms)
            {
                case "SQL Server":
                case "PostgreSQL":
                    return true;
                default:
                    Console.WriteLine("Аргумент команды -dbms указан неверно. " +
                        "Он может принимать только значение \"SQL Server\" или \"PostgreSQL\".");
                    return false;
            }
        }

        private bool ValidateConnectionString()
        {
            try
            {
                using (DbConnection connection = ConnectorFactory.CreateConnector(this).Connection)
                {
                    connection.Open();
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"При попытке соединения с {ConnectionString} произошла ошибка. " +
                    "Проверьте правильность строки подключения.");
                return false;
            }

            return true;
        }

        private bool ValidateTableName()
        {
            BaseConnector connector = ConnectorFactory.CreateConnector(this);

            // Проверяем наличие таблицы в SQL Server.
            if (connector is SqlServerConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    SqlCommand sqlQuery = (SqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                        "WHERE TABLE_NAME = @TableName) SELECT 1 ELSE SELECT 0";
                    sqlQuery.Parameters.AddWithValue("TableName", TableName);
                    if ((int)sqlQuery.ExecuteScalar() == 0)
                    {
                        Console.WriteLine($"Таблицы с именем {TableName} не существует. " +
                            "Проверьте правильность названия таблицы.");
                        return false;
                    }
                }

                return true;
            }

            // Проверяем наличие таблицы в PostgreSQL.
            if (connector is PostgreSqlConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    NpgsqlCommand sqlQuery = (NpgsqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "SELECT EXISTS(SELECT 1 FROM information_schema.tables " +
                        "WHERE table_name = @TableName);";
                    sqlQuery.Parameters.AddWithValue("TableName", TableName);
                    if (!(bool)sqlQuery.ExecuteScalar())
                    {
                        Console.WriteLine($"Таблицы с именем {TableName} не существует. " +
                            "Проверьте правильность названия таблицы.");
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private bool ValidatePrimaryKeyFieldName()
        {
            BaseConnector connector = ConnectorFactory.CreateConnector(this);

            // Проверяем наличие столбца первичного ключа в SQL Server.
            if (connector is SqlServerConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    SqlCommand sqlQuery = (SqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS " +
                        "WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @PrimaryKeyFieldName) SELECT 1 ELSE SELECT 0";
                    sqlQuery.Parameters.AddWithValue("TableName", TableName);
                    sqlQuery.Parameters.AddWithValue("PrimaryKeyFieldName", PrimaryKeyFieldName);
                    if ((int)sqlQuery.ExecuteScalar() == 0)
                    {
                        Console.WriteLine($"Столбца с именем {PrimaryKeyFieldName} в таблице {TableName} не существует. " +
                            "Проверьте правильность названия столбца первичного ключа.");
                        return false;
                    }
                }

                return true;
            }

            // Проверяем наличие столбца первичного ключа в PostgreSQL.
            if (connector is PostgreSqlConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    NpgsqlCommand sqlQuery = (NpgsqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "SELECT EXISTS(SELECT 1 FROM information_schema.columns " +
                        "WHERE table_name = @TableName AND column_name = @PrimaryKeyFieldName);";
                    sqlQuery.Parameters.AddWithValue("TableName", TableName);
                    sqlQuery.Parameters.AddWithValue("PrimaryKeyFieldName", PrimaryKeyFieldName);
                    if (!(bool)sqlQuery.ExecuteScalar())
                    {
                        Console.WriteLine($"Столбца с именем {PrimaryKeyFieldName} в таблице {TableName} не существует. " +
                            "Проверьте правильность названия столбца первичного ключа.");
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private bool ValidateUrlFieldName()
        {
            BaseConnector connector = ConnectorFactory.CreateConnector(this);

            // Проверяем наличие столбца Url в SQL Server.
            if (connector is SqlServerConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    SqlCommand sqlQuery = (SqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS " +
                        "WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @UrlFieldName) SELECT 1 ELSE SELECT 0";
                    sqlQuery.Parameters.AddWithValue("TableName", TableName);
                    sqlQuery.Parameters.AddWithValue("UrlFieldName", UrlFieldName);
                    if ((int)sqlQuery.ExecuteScalar() == 0)
                    {
                        Console.WriteLine($"Столбца с именем {UrlFieldName} в таблице {TableName} не существует. " +
                            "Проверьте правильность названия столбца Url.");
                        return false;
                    }
                }

                return true;
            }

            // Проверяем наличие столбца Url в PostgreSQL.
            if (connector is PostgreSqlConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    NpgsqlCommand sqlQuery = (NpgsqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "SELECT EXISTS(SELECT 1 FROM information_schema.columns " +
                        "WHERE table_name = @TableName AND column_name = @UrlFieldName);";
                    sqlQuery.Parameters.AddWithValue("TableName", TableName);
                    sqlQuery.Parameters.AddWithValue("UrlFieldName", UrlFieldName);
                    if (!(bool)sqlQuery.ExecuteScalar())
                    {
                        Console.WriteLine($"Столбца с именем {UrlFieldName} в таблице {TableName} не существует. " +
                            "Проверьте правильность названия столбца Url.");
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private bool ValidatePath()
        {
            DirectoryInfo directory = new DirectoryInfo(Path);

            // Проверяем наличие пути в файловой системе.
            if (directory == null || !directory.Exists)
            {
                Console.WriteLine($"Пути {Path} в файловой системе не существует. " +
                        "Проверьте правильность пути.");
                return false;
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
        /// Задает или возвращает название поля в таблице файлов 
        /// базы данных, которое является первичным ключом.
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
        /// Задает или возвращает название поля в таблице файлов базы данных,
        /// отвечающее за хранение относительного пути хранения файла.
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
        /// Задает или возвращает значение, указывающее,
        /// по какому пути необходимо удалять файлы.
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
        /// Задает или возвращает значение, указывающее, 
        /// необходимо ли выполнять логирование действий программы. 
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
        /// Задает или возвращает значение, указывающее, 
        /// необходимо ли запрашивать подтвержедние перед удалением. 
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
        /// Выводит полную справку о программе.
        /// </summary>
        public void Help()
        {
            Console.WriteLine("\nУтилита предназначена для удаления файлов и записей о них из базы данных, " +
                "на которые нет ссылок из других таблиц базы данных. При удалении файлов также выполняется " +
                "удаление файла с расширением .pdf с тем же именем. Утилита позволяет:\n" +
                "\n1. указать строку подключения к базе данных;" +
                "\n2. указать СУБД, в которой необходимо выполнить удаление;" +
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
                "\n\nЕсли установлен параметр, запрашивающий подтверждение об удалении, то перед удалением каждого файла будет появляться сообщение с просьбой нажатия соответствующей клавиши:\n\tA - удалить все файлы;\n\tY - удалить текущий файл;\n\tN - пропустить удаление текущего файла;\n\tE - пропустить удаление всех файлов.\n\n\n");
        }
    }
}
