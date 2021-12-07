namespace TelegramBot.Commands
{
    /// <summary>
    /// Класс команды останавливающей тренировку
    /// </summary>
    public class StopTrainingCommand : AbstractCommand, IChatTextCommandWithAction
    {
        public StopTrainingCommand()
        {
            base.CommandText = "/stop";
        }

        /// <summary>
        /// Выполняет остановку тренировки.
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public bool DoAction(Conversation chat)
        {
            chat.IsTraningInProcess = false;//выставляем флаг, разрешаем запуск новой тренировки
            return !chat.IsTraningInProcess;
        }

        /// <summary>
        /// Возвращает строку "Тренировка остановлена!"
        /// </summary>
        /// <returns></returns>
        public string ReturnText()
        {
            return "Тренировка остановлена!";
        }
    }
}