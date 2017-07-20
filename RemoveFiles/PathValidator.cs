using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, которая проверяет, существует ли путь к каталогу в файловой системе.
    /// </summary>
    class PathValidator : IValidator
    {
        /// <summary>
        /// Проверяет, существует ли путь к каталогу в файловой системе.
        /// </summary>
        /// <param name="command">Команда, необходимая для получения абсолютного пути к файлу.</param>
        /// <returns>true, если проверка прошла успешно, иначе false.</returns>
        public bool Validate(Command command)
        {
            DirectoryInfo directory = new DirectoryInfo(command.Path);

            // Проверяем наличие пути в файловой системе.
            if (directory == null || !directory.Exists)
            {
                Console.WriteLine($"Пути {command.Path} в файловой системе не существует. " +
                        "Проверьте правильность пути.");
                return false;
            }

            return true;
        }
    }
}
