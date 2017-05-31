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
    class Settings
    {
        Command command;

        private const string helpCommand = "-help";
        private const string connectionStringCommand = "-cs";
        private const string tableNameCommand = "-tn";
        private const string primaryKeyFieldNameCommand = "-pk";
        private const string urlFieldNameCommand = "-url";
        private const string pathCommand = "-path";
        private const string logCommand = "-log";
        private const string confirmationCommand = "-conf";

        /// <summary>
        /// Создает новый экземпляр класса Settings и устанавливает
        /// все параметры программы в значения по умолчанию.
        /// </summary>
        public Settings()
        {
            command = new Command();
        }

        /// <summary>
        /// Создает новый экземпляр класса Settings и устанавливает параметры программы
        /// в соответствии с переданным в функции Main значением args.
        /// </summary>
        /// <param name="args"></param>
        public Settings(string[] args)
        {
            command = new Command();
            this.ParseArgs(args);
        }

        /// <summary>
        /// Выполняет parsing переданных аргументов
        /// и присваивает их свойствам класса Command
        /// </summary>
        /// <param name="args"></param>
        private void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                // args[++i] означает аргумент команды, который должен следовать сразу за ней.
                switch (args[i])
                {
                    case helpCommand:
                        command.Help();
                        break;
                    case connectionStringCommand:
                        command.ConnectionString = args[++i];
                        break;
                    case tableNameCommand:
                        command.TableName = args[++i];
                        break;
                    case primaryKeyFieldNameCommand:
                        command.PrimaryKeyFieldName = args[++i];
                        break;
                    case urlFieldNameCommand:
                        command.UrlFieldName = args[++i];
                        break;
                    case pathCommand:
                        command.Path = args[++i];
                        break;
                    case logCommand:
                        command.Log = args[++i];
                        break;
                    case confirmationCommand:
                        command.Confirmation = args[++i];
                        break;
                    default:
                        Console.WriteLine("Аргументы заданы неверно. Выполните команду -help для получения дополнительной информации.");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает таблицу, содержащую названия таблиц, которые имеют 
        /// внешние ключи на таблицу файлов, и названия внешних ключей 
        /// из базы данных, указанной в строке подключения connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private DataTable GetTablesAndForeignKeys(SqlConnection connection)
        {
            // Получаем DataTable, содержащий названия таблиц и внешних ключей на таблицу файлов.
            SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText =
                @"SELECT 
                   OBJECT_NAME (fk.parent_object_id) TableName,
                   COL_NAME (fkc.parent_object_id, fkc.parent_column_id) ColumnName
                FROM 
                   sys.foreign_keys AS fk
                INNER JOIN 
                   sys.foreign_key_columns AS fkc ON fk.OBJECT_ID = fkc.constraint_object_id
                INNER JOIN 
                   sys.tables t ON t.OBJECT_ID = fkc.referenced_object_id
                WHERE 
                   OBJECT_NAME (fk.referenced_object_id) = @FilesTableName";
            sqlCommand.Parameters.Add("FilesTableName", SqlDbType.NVarChar).Value = command.TableName;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        /// <summary>
        /// Возвращает список внешних ключей (хранящихся в поле columnName) 
        /// таблицы tableName на таблицу файлов из базы данных, 
        /// указанной в строке подключения connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private List<object> GetForeignKeysFromTable(SqlConnection connection, string tableName, string columnName)
        {
            // Получаем список внешних ключей на таблицу файлов.
            SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {0} IS NOT NULL", columnName, tableName);

            List<object> foreignKeys = new List<object>();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    foreignKeys.Add(reader.GetValue(0));
                }
            }

            return foreignKeys;
        }

        /// <summary>
        /// Возвращает список первичных ключей таблицы файлов 
        /// из базы данных, указанной в строке подключения connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private List<object> GetFilesPrimaryKeys(SqlConnection connection)
        {
            // Получаем список первичных ключей таблицы файлов.
            SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1}", command.PrimaryKeyFieldName, command.TableName);

            List<object> primaryKeys = new List<object>();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    primaryKeys.Add(reader.GetValue(0));
                }
            }

            return primaryKeys;
        }

        /// <summary>
        /// Удаляет запись о файле с первичным ключом primaryKey 
        /// из базы данных, указанной в строке подключения connection, 
        /// и соответствующий записи файл из файловой системы.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key"></param>
        private void RemoveRecordAndFile(SqlConnection connection, object primaryKey)
        {
            // Удаляем файлы (включая .pdf) из файловой системы.
            SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName, primaryKey);
            string relativePath = (string)sqlCommand.ExecuteScalar();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", "Name", command.TableName, command.PrimaryKeyFieldName, primaryKey);
            string filename = (string)sqlCommand.ExecuteScalar();
            DirectoryInfo directory = new DirectoryInfo(command.Path + relativePath);
            FileInfo[] files = directory.GetFiles(filename + ".*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            // Удаляем запись файла из базы данных.
            sqlCommand.CommandText = string.Format("DELETE FROM {0} WHERE {1} = {2}", command.TableName, command.PrimaryKeyFieldName, primaryKey);
            sqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Запрашивает подтверждение на удаление файлов,
        /// если аргумент -conf установлен в true.
        /// </summary>
        /// <returns></returns>
        private bool RequestConfirmation()
        {
            if (command.Confirmation == "true")
            {
                Console.WriteLine("Вы уверены, что хотите удалить файлы? (Y/N)");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Удаление файлов отменено.");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Выполняет команду удаления записей о файлах из базы данных
        /// и соответствующих файлов из файловой системы
        /// с учетом переданных во время вызова параметров.
        /// </summary>
        public void Execute()
        {
            if (RequestConfirmation())
            {
                using (SqlConnection connection = new SqlConnection(command.ConnectionString))
                {
                    connection.Open();

                    // Получаем названия таблиц и внешних ключей на таблицу файлов.
                    DataTable tablesAndForeignKeysNames = GetTablesAndForeignKeys(connection);

                    // Получаем список ключей файлов, на которые есть ссылки из других таблиц.
                    List<object> keysWithRefs = new List<object>();
                    foreach (DataRow row in tablesAndForeignKeysNames.Rows)
                    {
                        // Получаем список внешних ключей из указанной таблицы.
                        List<object> foreignKeys = GetForeignKeysFromTable(connection, (string)row["TableName"], (string)row["ColumnName"]);

                        // Собираем внешние ключи в список так, чтобы они не повторялись.
                        foreach (object foreignKey in foreignKeys)
                        {
                            if (!keysWithRefs.Contains(foreignKey))
                            {
                                keysWithRefs.Add(foreignKey);
                            }
                        }
                    }

                    // Получаем список первичных ключей таблицы файлов.
                    List<object> primaryKeys = GetFilesPrimaryKeys(connection);

                    // Определяем записи, на которые нет ссылок и удаляем их.
                    foreach (object primaryKey in primaryKeys)
                    {
                        if (!keysWithRefs.Contains(primaryKey))
                        {
                            RemoveRecordAndFile(connection, primaryKey);
                        }
                    }
                }

                Console.WriteLine("Удаление файлов прошло успешно.");
            }
        }
    }
}
