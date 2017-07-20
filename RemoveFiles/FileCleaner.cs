using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность для удаления файлов из файловой системы и записей из базы данных (контекст).
    /// </summary>
    class FileCleaner
    {
        private BaseConnector Connector { get; set; }
        private Command Command { get; set; }

        /// <summary>
        /// Создает новый экземпляр класса FileCleaner и создает коннектор к СУБД.
        /// </summary>
        /// <param name="command">Команда, необходимая для создания коннектора к СУБД.</param>
        public FileCleaner(Command command)
        {
            Command = command;
            Connector = ConnectorFactory.CreateConnector(Command);
        }

        /// <summary>
        /// Выполняет удаление файлов из файловой системы и записей из базы данных.
        /// </summary>
        public void Clean()
        {
            Connector.Execute(Command);
        }
    }
}
