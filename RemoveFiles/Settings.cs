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
        // TODO: Комментарии должны быть у всех полей, свойств и методов класса, которые не являются совершенно очевидными. 
        // В данном случае command - слишком общий термин, чтоюы являться очевидным.
        Command command;
        
        // TODO: Нужны комментарии, поясняющий, что это допустимые параметр командной строки и что он означает.
        internal const string HelpCommand = "-help";
        internal const string ConnectionStringCommand = "-cs";
        internal const string TableNameCommand = "-tn";
        internal const string PrimaryKeyFieldNameCommand = "-pk";
        internal const string UrlFieldNameCommand = "-url";
        internal const string PathCommand = "-path";
        internal const string LogCommand = "-log";
        internal const string ConfirmationCommand = "-conf";

        /// <summary>
        /// Создает новый экземпляр класса Settings и устанавливает параметры программы
        /// в соответствии с переданным в функции Main значением args.
        /// </summary>
        /// <param name="args">//TODO: Комментарии должны быть полными.</param>
        public Settings(string[] args)
        {
            Logger.InitLogger();
            command = this.ParseArgs(args);
        }

        /// <summary>
        /// Конструктор для тестов.
        /// </summary>
        public Settings()
        { }

        /// <summary>
        /// Выполняет parsing переданных аргументов
        /// и присваивает их свойствам класса Command
        /// </summary>
        /// <param name="args">//TODO: Комментарии должны быть полными.</param>
        internal Command ParseArgs(string[] args)
        {
            var command = new Command();
            for (int i = 0; i < args.Length; i++)
            {
                // args[++i] означает аргумент команды, который должен следовать сразу за ней.
                switch (args[i])
                {
                    case HelpCommand:
                        command.Help();
                        break;
                    case ConnectionStringCommand:
                        command.ConnectionString = args[++i];
                        break;
                    case TableNameCommand:
                        command.TableName = args[++i];
                        break;
                    case PrimaryKeyFieldNameCommand:
                        command.PrimaryKeyFieldName = args[++i];
                        break;
                    case UrlFieldNameCommand:
                        command.UrlFieldName = args[++i];
                        break;
                    case PathCommand:
                        command.Path = args[++i];
                        break;
                    case LogCommand:
                        command.Log = args[++i];
                        break;
                    case ConfirmationCommand:
                        command.Confirmation = args[++i];
                        break;
                    default:
                        throw new ArgumentException("Аргументы заданы неверно. Выполните команду -help для получения дополнительной информации.");                                                
                }
            }
            return command;
        }

        /// <summary>
        /// Возвращает таблицу, содержащую названия таблиц, которые имеют 
        /// внешние ключи на таблицу файлов, и названия внешних ключей 
        /// из базы данных, указанной в строке подключения connection.
        /// </summary>
        /// <param name="connection">//TODO: Комментарии должны быть полными.</param>        
        /// <returns>//TODO: Комментарии должны быть полными.</returns>
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
        /// <param name="connection">//TODO: Комментарии должны быть полными.</param>
        /// <param name="tableName">//TODO: Комментарии должны быть полными.</param>
        /// <param name="columnName">//TODO: Комментарии должны быть полными.</param>
        /// <returns>//TODO: Комментарии должны быть полными.</returns>
        private List<object> GetForeignKeysFromTable(SqlConnection connection, string tableName, string columnName)
        {
            // Получаем список внешних ключей на таблицу файлов.
            SqlCommand sqlCommand = connection.CreateCommand();
            //TODO: Запрос в таком виде может вызвать ошибку. Необходимо поместить имена полей и таблиц в квадратные скобки. 
            // Например, если имя поля будет user, такой запрос выполнится некорректно.            
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
        /// <param name="connection">//TODO: Комментарии должны быть полными.</param>
        /// <returns>//TODO: Комментарии должны быть полными.</returns>
        private List<object> GetFilesPrimaryKeys(SqlConnection connection)
        {
            // Получаем список первичных ключей таблицы файлов.
            SqlCommand sqlCommand = connection.CreateCommand();
            //TODO: Поместить имена полей и таблиц в квадратные скобки.
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
        /// Возвращает строковое представление записи в базе данных,
        /// указанной в строке подключения connection.
        /// </summary>
        /// <returns>//TODO: Комментарии должны быть полными.</returns>
        private string GetRemovingRecord(SqlConnection connection, object primaryKey)
        {
            // Запрашиваем запись удаляемого файла.
            SqlCommand sqlCommand = connection.CreateCommand();
            //TODO: Поместить имена полей и таблиц в квадратные скобки.            
            //TODO: Нет ли здесь ошибки в формировании запроса? На мой взгляд нужно обернуть параметр {2} в кавычки. Иначе произвольная строка с пробелом все испортит. И не только строка.
            sqlCommand.CommandText = string.Format("SELECT * FROM {0} WHERE {1} = {2}", command.TableName, command.PrimaryKeyFieldName, primaryKey);
            //TODO: Насколько я помню адаптер является IDisposable, поэтому его нужно правильно закрывать. Проще всего использовать using().
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            // Собираем поля записи в строку.
            string result = string.Empty;
            foreach (DataColumn column in table.Columns)
            {
                // TODO: Складывать строки в цикле неизвестной длины - как правило очень плохая практика. Нужно использовать StringBuilder. Рекомендую для ознакомления http://jonskeet.uk/csharp/stringbuilder.html
                result += string.Format("{0}: {1}, ", column.ColumnName, table.Rows[0][column].ToString());
            }
            
            return result.Trim().Trim(',');
        }

        /// <summary>
        /// Удаляет запись о файле с первичным ключом primaryKey 
        /// из базы данных, указанной в строке подключения connection, 
        /// и соответствующий записи файл из файловой системы.
        /// </summary>
        /// <param name="connection">//TODO: Комментарии должны быть полными.</param>
        /// <param name="key">//TODO: Комментарии должны быть полными.</param>
        private void RemoveRecordAndFile(SqlConnection connection, object primaryKey)
        {
            // Переменные для логирования.
            string removedFile = "Удален файл: ";
            string removedRecord = "Удалена запись: ";

            // Удаляем файлы (включая .pdf) из файловой системы.
            //TODO: Имя файла в файловой системе не содержится в отдельном поле таблицы, оно входит в относительный путь. Поэтому достаточно вычитать его из БД и выделить из него имя файла без расширения.
            SqlCommand sqlCommand = connection.CreateCommand();
            //TODO: Нет ли здесь ошибки в формировании запроса? На мой взгляд нужно обернуть параметр {3} в кавычки. Иначе произвольная строка с пробелом все испортит. И не только строка.
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", command.UrlFieldName, command.TableName, command.PrimaryKeyFieldName, primaryKey);
            string relativePath = (string)sqlCommand.ExecuteScalar();
            sqlCommand.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", "Name", command.TableName, command.PrimaryKeyFieldName, primaryKey);
            string filename = (string)sqlCommand.ExecuteScalar();
            DirectoryInfo directory = new DirectoryInfo(command.Path + relativePath);
            FileInfo[] files = directory.GetFiles(filename + ".*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files)
            {
                removedFile += file.FullName + "\n";
                file.Delete();
            }

            removedRecord += GetRemovingRecord(connection, primaryKey) + "\n";

            // Удаляем запись файла из базы данных.
            //TODO: Нет ли здесь ошибки в формировании запроса? На мой взгляд нужно обернуть параметр {2} в кавычки. Иначе произвольная строка с пробелом все испортит. И не только строка.
            sqlCommand.CommandText = string.Format("DELETE FROM {0} WHERE {1} = {2}", command.TableName, command.PrimaryKeyFieldName, primaryKey);
            sqlCommand.ExecuteNonQuery();

            // Выполняем логирование.
            if (command.Log == "true")
            {
                Logger.Log.InfoFormat(removedFile);
                Logger.Log.InfoFormat(removedRecord);
            }
        }

        /// <summary>
        /// Запрашивает подтверждение на удаление файлов,
        /// если аргумент -conf установлен в true.
        /// </summary>
        /// <returns>//TODO: Комментарии должны быть полными.</returns>
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
            //TODO: Довольно странно задавать такой вопрос сразу после запуска приложения, когда еще даже не подсчитано количество файлов для удаления. Может и удалять ничего не понадобится. 
            // Предлагаю показывать его после подсчета количества файлов, которые будут удалены, предупреждая пользователя об этом количестве.
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
                            //TODO: В данном случае действие производится за O(N), что для больших таблиц не лучший вариант. Однако есть более эффективная структура для этих целей - HashSet<>. Предлагаю использовать её.
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
