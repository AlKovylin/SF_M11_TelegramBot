using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Контракт предписывающий реализацию методов: создания клавиатуры, выполнения подписки на
    /// событие нажатия кнопок клавиатуры пользователем чата, возвращающей сообщение поясняющее назначение клавиатуры.
    /// InlineKeyboardMarkup ReturnKeyBoard(); void AddCallBack(Conversation chat); string InformationalMessage();
    /// </summary>
    interface IKeyBoardCommand
    {
        /// <summary>
        /// Функция возвращающая объект клавиатуры.
        /// </summary>
        /// <returns></returns>
        InlineKeyboardMarkup ReturnKeyBoard();
        /// <summary>
        /// Функция выполняющая подписку на событие нажатия кнопок клавиатуры.
        /// </summary>
        /// <param name="chat"></param>
        void AddCallBack(Conversation chat);
        /// <summary>
        /// Проверяет возможность выполнения команды
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        bool CheckPossibility(Conversation chat);        
        /// <summary>
        /// Функция возвращающая информационное сообщение - пояснение к клавиатуре.
        /// </summary>
        /// <returns></returns>
        string InformationalMessage();
    }
}