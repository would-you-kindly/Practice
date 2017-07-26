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
    /// Представляет сущность, которая проверяет, существует ли таблица с данным названием.
    /// </summary>
    class TableNameValidator : IValidator
    {
        /// <summary>
        /// Проверяет, существует ли таблица с данным названием.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения названия таблицы файлов.</param>
        /// <returns>true, если проверка прошла успешно, иначе false.</returns>
        public bool Validate(Command command)
        {
            string error = $"Таблицы с именем \"{command.TableName}\" не существует. " +
                "Проверьте правильность названия таблицы.";

            BaseConnector connector = ConnectorFactory.CreateConnector(command);

            // Проверяем наличие таблицы в SQL Server.
            if (connector is SqlServerConnector)
            {
                using (connector.Connection)
                {
                    connector.Connection.Open();
                    SqlCommand sqlQuery = (SqlCommand)connector.Connection.CreateCommand();
                    sqlQuery.CommandText = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                        "WHERE TABLE_NAME = @TableName) SELECT 1 ELSE SELECT 0";
                    sqlQuery.Parameters.AddWithValue("TableName", command.TableName);
                    if ((int)sqlQuery.ExecuteScalar() == 0)
                    {
                        Logger.Log.ErrorFormat(error);
                        Console.WriteLine(error);
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
                    sqlQuery.Parameters.AddWithValue("TableName", command.TableName);
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
