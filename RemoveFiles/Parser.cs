using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles
{
    /// <summary>
    /// Представляет сущность для parsing'а аргументов программы.
    /// </summary>
    class Parser
    {
        /// <summary>
        /// Выполняет parsing переданных аргументов и присваивает их свойствам класса Command.
        /// </summary>
        /// <param name="args">Набор аргументов программы.</param>
        public Command ParseArgs(string[] args)
        {
            Command command = new Command();

            for (int i = 0; i < args.Length; i++)
            {
                // Команды, которые принимают аргумент, не могут быть последним элементом в args.
                if (i == args.Length - 1 && 
                    (args[i] == Settings.DbmsCommand || 
                    args[i] == Settings.ConnectionStringCommand ||
                    args[i] == Settings.TableNameCommand ||
                    args[i] == Settings.PrimaryKeyFieldNameCommand ||
                    args[i] == Settings.UrlFieldNameCommand ||
                    args[i] == Settings.PathCommand))
                {
                    throw new ArgumentException($"Для команды {args[i]} не задан параметр. Выполните команду -help для получения дополнительной информации.");
                }

                // args[++i] означает аргумент команды, который должен следовать сразу за ней.
                switch (args[i])
                {
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
                        throw new ArgumentException($"Команды {args[i]} не существует. Выполните команду -help для получения дополнительной информации.");
                }
            }

            if (!command.Validate())
            {
                throw new ArgumentException("Аргументы заданы неверно. Выполните команду -help для получения дополнительной информации.");
            }

            return command;
        }
    }
}
