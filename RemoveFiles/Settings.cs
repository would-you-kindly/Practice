using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RemoveFiles
{
    class Settings
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

        //private List<string> commands = new List<string>() { "-help", "-cs", "-tn", "-pk", "-url", "-path", "-log", "-conf" };
        private Dictionary<Commands, string> commands = new Dictionary<Commands, string>()
        {
            { Commands.Help, "-help" },
            { Commands.ConnectionString, "-cs" },
            { Commands.TableName, "-tn" },
            { Commands.PrimaryKeyFieldName, "-pk" },
            { Commands.UrlFieldName, "-url" },
            { Commands.Path, "-path" },
            { Commands.Log, "-log" },
            { Commands.Confirmation, "-conf" }
        };

        private const string defaultConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=FlexberryPractice;Integrated Security=True;";
        private const string defaultTableName = "Files";
        private const string defaultPrimaryKeyFieldName = "PrimaryKey";
        private const string defaultUrlFieldName = "Url";
        private const string defaultPath = @"C:\";
        private const bool defaultLog = true;
        private const bool defaultConfirmation = true;

        private string connectionString;
        private string tableName;
        private string primaryKeyFieldName;
        private string urlFieldName;
        private string path;
        private bool log;
        private bool confirmation;

        private bool isArgsCorrect = false;

        private Dictionary<DataRow, bool> fileReferences = new Dictionary<DataRow, bool>();

        /// <summary>
        /// Создает новый экземпляр класса Settings и устанавливает
        /// все параметры программы в значения по умолчанию.
        /// </summary>
        public Settings()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Создает новый экземпляр класса Settings и устанавливает параметры программы
        /// в соответствии с переданным в функции Main значением args.
        /// </summary>
        /// <param name="args"></param>
        public Settings(string[] args)
        {
            this.ParseArgs(args);
        }

        private void ParseArgs(string[] args)
        {
            if (args.Length != 0)
            {
                foreach (string arg in args)
                {
                    if (arg == commands[Commands.Help])
                    {
                        Console.WriteLine("Вывод справки");
                    }
                }
            }
            else
            {
                this.SetDefaults();
            }
        }

        /// <summary>
        /// Устанавливает значения по умолчанию для всех параметров программы.
        /// </summary>
        private void SetDefaults()
        {
            this.connectionString = defaultConnectionString;
            this.tableName = defaultTableName;
            this.primaryKeyFieldName = defaultPrimaryKeyFieldName;
            this.urlFieldName = defaultUrlFieldName;
            this.path = defaultPath;
            this.log = defaultLog;
            this.confirmation = defaultConfirmation;

            this.isArgsCorrect = true;
        }

        /// <summary>
        /// Возвращает DataSet, содержащий данные всех таблиц базы данных,
        /// указанной в строке подключения connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private DataSet GetDataSet(SqlConnection connection)
        {
            // Получаем DataTable, содержащий записи о таблицах текущей базы данных.
            SqlCommand command = new SqlCommand("select * from sys.tables", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            adapter.FillSchema(table, SchemaType.Source);

            // Получаем DataSet, содержащий все таблицы с данными текущей базы данных.
            DataSet dataSet = new DataSet();
            foreach (DataRow row in table.Rows)
            {
                command = new SqlCommand("select * from " + row["Name"].ToString(), connection);
                adapter = new SqlDataAdapter(command);
                table = new DataTable(row["Name"].ToString());
                adapter.Fill(table);
                dataSet.Tables.Add(table);
            }

            dataSet.WriteXmlSchema(@"..\..\DataSet1.xsd");

            return dataSet;
        }

        public void Execute()
        {
            if (this.isArgsCorrect)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataSet dataSet = GetDataSet(connection);

                    // Здесь должны быть связи, но они не загрузились
                    foreach (DataRelation relation in dataSet.Relations)
                    {
                        Console.WriteLine(relation.RelationName);
                    }
                }

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Аргументы заданы неверно. Выполните -help для получения дополнительной информации.");
            }
        }
    }
}
