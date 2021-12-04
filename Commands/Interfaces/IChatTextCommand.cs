namespace TelegramBot
{
    /// <summary>
    /// Контракт предписывающий реализацию функции возвращающей строку в ответ на полученную команду.
    /// string ReturnText();
    /// </summary>
    interface IChatTextCommand
    {
        /// <summary>
        /// Возвращает текстовое сообщение.
        /// </summary>
        /// <returns></returns>
        string ReturnText();
    }
}