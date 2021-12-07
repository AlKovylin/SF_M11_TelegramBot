namespace TelegramBot
{
    /// <summary>
    /// Класс команды в ответ на которую бот присылает сообщение "Привет!"
    /// </summary>
    public class SayHiCommand : AbstractCommand, IChatTextCommand
    {
        /// <summary>
        /// Конструктор. Присваивает полю значение команды.
        /// </summary>
        public SayHiCommand()
        {
            base.CommandText = "/saymehi";
        }

        /// <summary>
        /// Возвращает текстовое сообщение.
        /// </summary>
        /// <returns>"Привет!"</returns>
        public string ReturnText()
        {
            return "Привет!";
        }
    }
}