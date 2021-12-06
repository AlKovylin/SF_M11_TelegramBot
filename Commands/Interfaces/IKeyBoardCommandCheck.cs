using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Контракт. Дополнительно предписывает командам, имеющим клавиатуру,
    /// иметь метод проверяющий возможность использования клавиатуры и метод возвращающий сообщение если это не возможно. 
    /// bool DoAction(Conversation chat)
    /// </summary>
    interface IKeyBoardCommandCheck
    {
        /// <summary>
        /// Проверяет возможность выполнения команды
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        bool CheckPossibility(Conversation chat);
        /// <summary>
        /// Возвращает сообщение если использование клавиатуры не возможно.
        /// </summary>
        /// <returns></returns>
        string ReturnErrText();
    }
}
