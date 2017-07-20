using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность, проверяющих на корректность аргументы программы.
    /// </summary>
    interface IValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">Команда, необходима для получения аргументов программы.</param>
        /// <returns>true, если проверка прошла успешно, иначе false.</returns>
        bool Validate(Command command);
    }
}
