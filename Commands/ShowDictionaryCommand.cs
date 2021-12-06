using System;
using System.Collections.Generic;
using System.Text;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Класс команды возвращающей в чат словарь конкретного чата.
    /// </summary>
    public class ShowDictionaryCommand : AbstractCommand, IChatDictionaryCommand
    {
        /// <summary>
        /// Конструктор. Присваивает полю значение команды.
        /// </summary>
        public ShowDictionaryCommand()
        {
            CommandText = "/dictionary";
        }

        public Dictionary<string, Word> ReturnDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
