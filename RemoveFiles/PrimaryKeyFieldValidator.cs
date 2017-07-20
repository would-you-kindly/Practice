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
    /// Представляет сущность, которая проверяет, существует ли поле первичного ключа с данным названием.
    /// </summary>
    class PrimaryKeyFieldValidator : IValidator
    {
        /// <summary>
        /// Проверяет, существует ли поле первичного ключа с данным названием.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения названия поля первичного ключа.</param>
        /// <returns>true, если проверка прошла успешно, иначе false.</returns>
        public bool Validate(Command command)
        {
            BaseConnector connector = ConnectorFactory.CreateConnector(command);

            // Проверяем наличие столбца первичного ключа в SQL Server.
            if (connector is SqlServerConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    SqlCommand sqlQuery = (SqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS " +
                        "WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @PrimaryKeyFieldName) SELECT 1 ELSE SELECT 0";
                    sqlQuery.Parameters.AddWithValue("TableName", command.TableName);
                    sqlQuery.Parameters.AddWithValue("PrimaryKeyFieldName", command.PrimaryKeyFieldName);
                    if ((int)sqlQuery.ExecuteScalar() == 0)
                    {
                        Console.WriteLine($"Столбца с именем {command.PrimaryKeyFieldName} в таблице {command.TableName} не существует. " +
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
                    sqlQuery.Parameters.AddWithValue("TableName", command.TableName);
                    sqlQuery.Parameters.AddWithValue("PrimaryKeyFieldName", command.PrimaryKeyFieldName);
                    if (!(bool)sqlQuery.ExecuteScalar())
                    {
                        Console.WriteLine($"Столбца с именем {command.PrimaryKeyFieldName} в таблице {command.TableName} не существует. " +
                            "Проверьте правильность названия столбца первичного ключа.");
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
