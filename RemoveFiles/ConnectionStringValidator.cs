using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, которая проверяет на корректность указанную строку подключения.
    /// </summary>
    class ConnectionStringValidator : IValidator
    {
        /// <summary>
        /// Проверяет на корректность указанную строку подключения.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения строки подключения к базе данных.</param>
        /// <returns>true, если проверка прошла успешно, иначе false.</returns>
        public bool Validate(Command command)
        {
            string error = $"При попытке соединения с {command.ConnectionString} произошла ошибка. " +
                "Проверьте правильность строки подключения.";

            try
            {
                using (DbConnection connection = ConnectorFactory.CreateConnector(command).Connection)
                {
                    connection.Open();
                }
            }
            catch (Exception)
            {
                Logger.Log.ErrorFormat(error);
                Console.WriteLine(error);
                return false;
            }

            return true;
        }
    }
}
