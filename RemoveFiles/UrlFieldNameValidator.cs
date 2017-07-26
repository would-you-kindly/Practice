using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, которая проверяет, существует ли поле относительного пути с данным названием.
    /// </summary>
    class UrlFieldNameValidator : IValidator
    {
        /// <summary>
        /// Проверяет, существует ли поле относительного пути с данным названием.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения названия поля с относительным путем файла.</param>
        /// <returns>true, если проверка прошла успешно, иначе false.</returns>
        public bool Validate(Command command)
        {
            string error = $"Столбца с именем \"{command.UrlFieldName}\" в таблице \"{command.TableName}\" не существует. " +
                "Проверьте правильность названия столбца Url.";

            BaseConnector connector = ConnectorFactory.CreateConnector(command);

            // Проверяем наличие столбца Url в SQL Server.
            if (connector is SqlServerConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    SqlCommand sqlQuery = (SqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS " +
                        "WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @UrlFieldName) SELECT 1 ELSE SELECT 0";
                    sqlQuery.Parameters.AddWithValue("TableName", command.TableName);
                    sqlQuery.Parameters.AddWithValue("UrlFieldName", command.UrlFieldName);
                    if ((int)sqlQuery.ExecuteScalar() == 0)
                    {
                        Logger.Log.ErrorFormat(error);
                        Console.WriteLine(error);
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
                    sqlQuery.Parameters.AddWithValue("TableName", command.TableName);
                    sqlQuery.Parameters.AddWithValue("UrlFieldName", command.UrlFieldName);
                    if (!(bool)sqlQuery.ExecuteScalar())
                    {
                        Logger.Log.ErrorFormat(error);
                        Console.WriteLine(error);
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
