namespace TelegramBot.Commands
{
    /// <summary>
    /// Класс команды удаления слова из библиотеки.
    /// </summary>
    public class DeleteWordCommand : AbstractChatTextCommandOption, IChatTextCommandWithAction
    {
        /// <summary>
        /// Конструктор. Присваивает полю значение команды.
        /// </summary>
        public DeleteWordCommand()
        {
            CommandText = "/deleteword";
        }
        /// <summary>
        /// Выполняет удаление слова из библиотеки.
        /// </summary>
        /// <param name="chat">Чат.</param>
        /// <returns>True если успешно и false, если поиск элемента в dictionary по ключу не увенчался успехом.</returns>
        public bool DoAction(Conversation chat)
        {
            var message = chat.GetLastMessage();

            var text = ClearMessageFromCommand(message);

            if (chat.dictionary.ContainsKey(text))
            {
                chat.dictionary.Remove(text);
                return true;
            }
            return false; 
        }
        /// <summary>
        /// Возвращает сообщение об успешном выполнении команды.
        /// </summary>
        /// <returns>"Слово успешно удалено!"</returns>
        public string ReturnText()
        { 
            return "Слово успешно удалено!";
        }
    }
}