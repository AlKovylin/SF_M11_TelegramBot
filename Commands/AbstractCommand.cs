namespace TelegramBot
{
    /// <summary>
    /// Базовый класс простых входящих команд.
    /// </summary>
    public abstract class AbstractCommand : IChatCommand
    {
        /// <summary>
        /// Поле хранящее текст команды.
        /// </summary>
        public string CommandText;
        /// <summary>
        /// Проверяет соответствие входящей команды.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>True/false</returns>
        public bool CheckMessage(string message)
        {
            return CommandText == message;
        }
    }
}
