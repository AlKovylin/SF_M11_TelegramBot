using System.Collections.Generic;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Контракт предписывающий реализацию функции возвращающей словарь в ответ на полученную команду.
    /// string ReturnDictionary();
    /// </summary>
    interface IChatDictionaryCommand
    {
        /// <summary>
        /// Возвращает словарь.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Word> ReturnDictionary();
    }
}
