using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    class Parser
    {
        /// <summary>
        /// Выполняет parsing переданных аргументов
        /// и присваивает их свойствам класса Command.
        /// </summary>
        /// <param name="args"></param>
        public Command ParseArgs(string[] args)
        {
            Command command = new Command();

            for (int i = 0; i < args.Length; i++)
            {
                if (i == args.Length - 1 && args[i] != "-help")
                {
                    throw new ArgumentException("Аргументы заданы неверно. Выполните команду -help для получения дополнительной информации.");
                }

                // args[++i] означает аргумент команды, который должен следовать сразу за ней.
                switch (args[i])
                {
                    // TODO: Разный порядок передачи значений может работать по-разному
                    case Settings.HelpCommand:
                        command.Help();
                        break;
                    case Settings.ConnectionStringCommand:
                        command.ConnectionString = args[++i];
                        break;
                    case Settings.DbmsCommand:
                        command.Dbms = args[++i];
                        break;
                    case Settings.TableNameCommand:
                        command.TableName = args[++i];
                        break;
                    case Settings.PrimaryKeyFieldNameCommand:
                        command.PrimaryKeyFieldName = args[++i];
                        break;
                    case Settings.UrlFieldNameCommand:
                        command.UrlFieldName = args[++i];
                        break;
                    case Settings.PathCommand:
                        command.Path = args[++i];
                        break;
                    case Settings.LogCommand:
                        command.Log = true;
                        break;
                    case Settings.ConfirmationCommand:
                        command.Confirmation = true;
                        break;
                    default:
                        throw new ArgumentException("Аргументы заданы неверно. Выполните команду -help для получения дополнительной информации.");
                }
            }

            return command;
        }
    }
}
