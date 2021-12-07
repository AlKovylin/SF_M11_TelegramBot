using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Контракт. Предписывает командам, иметь метод проверяющий возможность
    /// выполнения команды по дополнительным параметрам. И метод возвращающий
    /// информационное сообщение о причине.
    /// bool CheckPossibility(Conversation chat); string ReturnErrText();
    /// </summary>
    interface ICommandCheck
    {
        /// <summary>
        /// Проверяет возможность выполнения команды
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        bool CheckPossibility(Conversation chat);

        /// <summary>
        /// Возвращает сообщение о причине невозможности выполнения.
        /// </summary>
        /// <returns></returns>
        string ReturnErrText();
    }
}
