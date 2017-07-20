using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, которая проверяет на корректность указанную СУБД.
    /// </summary>
    class DbmsValidator : IValidator
    {
        /// <summary>
        /// Проверяет на корректность указанную СУБД.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения названия СУБД.</param>
        /// <returns>true, если проверка прошла успешно, иначе false.</returns>
        public bool Validate(Command command)
        {
            switch (command.Dbms)
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
    }
}
