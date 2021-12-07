namespace TelegramBot.Commands
{
    /// <summary>
    /// Базовый класс составных входящих команд.
    /// Позволяет обрабатывать составные команды (Пример: /deleteword стол).
    /// </summary>
    public abstract class AbstractChatTextCommandOption : IChatCommand
    {
        /// <summary>
        /// Поле хранящее текст команды.
        /// </summary>
        public string CommandText;

        /// <summary>
        /// Проверяет соответствует ли начало экземпляра полученной строки зарегистрированной команде.
        /// </summary>
        /// <param name="message">Сообщение из чата для проверки.</param>
        /// <returns></returns>
        public bool CheckMessage(string message)
        {
            return message.StartsWith(CommandText);
        }

        /// <summary>
        /// Извлекает подстроку из экземпляра полученной строки. В контексте данной программы 
        /// происходит отсечение команды и пробела после нёё. 
        /// </summary>
        /// <param name="message">Сообщение из чата для проверки.</param>
        /// <returns>Строка следующая за командой.</returns>
        public string ClearMessageFromCommand(string message)
        {
            return message.Substring(CommandText.Length + 1);//+1 - пробел
        }
    }
}
