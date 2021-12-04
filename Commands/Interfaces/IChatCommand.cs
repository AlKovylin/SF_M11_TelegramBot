namespace TelegramBot
{
    /// <summary>
    /// Контракт предписывающий реализацию функции проверяющей текст входящей команды.
    /// bool CheckMessage(string message);
    /// </summary>
    public interface IChatCommand
    {
        /// <summary>
        /// Проверяет соответствие входящей команды.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool CheckMessage(string message);
    }
}
