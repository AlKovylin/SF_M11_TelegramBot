using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Commands;

namespace TelegramBot
{
    /// <summary>
    /// Класс анализатор команд.
    /// </summary>
    public class CommandParser
    {
        /// <summary>
        /// Список объектов команд бота.
        /// </summary>
        private List<IChatCommand> Command;

        /// <summary>
        /// Поле класса контроллера добавления слова в словарь.
        /// </summary>
        private AddingController addingController;

        /// <summary>
        /// Конструктор. Инициализирует список команд бота и AddingController.
        /// </summary>
        public CommandParser()
        {
            Command = new List<IChatCommand>();
            addingController = new AddingController();
        }

        /// <summary>
        /// Добавляет объект команды в список объектов команд.
        /// </summary>
        /// <param name="chatCommand"></param>
        public void AddCommand(IChatCommand chatCommand)
        {
            Command.Add(chatCommand);
        }

        /// <summary>
        /// Проверяет наличие команды в списке. 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsMessageCommand(string message)
        {
            return Command.Exists(x => x.CheckMessage(message));
        }

        /// <summary>
        /// Проверяет реализует ли данная команда IChatTextCommand.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsTextCommand(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message));//ищем по списку соответствующую команду

            return command is IChatTextCommand;//является ли command объектом типа IChatTextCommand?
        }

        /// <summary>
        /// Проверяет реализует ли данная команда IKeyBoardCommand.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsButtonCommand(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message));

            return command is IKeyBoardCommand;
        }

        /// <summary>
        /// Проверяет реализует ли данная команда IKeyBoardCommandCheck.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsButtonCommandCheck(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message));

            return command is ICommandCheck;
        }

        /// <summary>
        /// Проверяет возможность выполнения команды по дополнительным критериям.
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public bool ButtonCommandCheckPossibility(string message, Conversation chat)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as ICommandCheck;

            return command.CheckPossibility(chat);
        }

        /// <summary>
        /// Возвращает сообщение если использование клавиатуры не возможно.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        public string GetErrCheckText(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as ICommandCheck;

            return command.ReturnErrText();
        }

        /// <summary>
        /// Возвращает из объекта команды, выполняющей конкретное действие, текстовое 
        /// сообщение об успешности выполнения команды, либо об ошибке.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        public string GetMessageText(string message, Conversation chat)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IChatTextCommand;

            if (command is IChatTextCommandWithAction)
            {
                if (!(command as IChatTextCommandWithAction).DoAction(chat))
                {
                    return "Ошибка выполнения команды!";
                };
            }

            return command.ReturnText();
        }

        /// <summary>
        /// Возвращает из обекта команды сообщение - пояснение к клавиатуре.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string GetInformationalMeggase(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command.InformationalMessage();
        }

        /// <summary>
        /// Возвращает из объекта команды клавиатуру.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public InlineKeyboardMarkup GetKeyBoard(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command.ReturnKeyBoard();
        }

        /// <summary>
        /// Обеспечивает объекту команды подписку на событие нажатия кнопки.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chat"></param>
        public void AddCallback(string message, Conversation chat)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;
            command.AddCallBack(chat);
        }

        /// <summary>
        /// Проверяет соответствует ли полученное сообщение коменде добавления нового слова в словарь.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsAddingCommand(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message));//ищем по списку

            return command is AddWordCommand;//проверяем является ли найденный объект нужным типом
        }

        /// <summary>
        /// Запускает процедуру ввода нового слова в словарь конкретного чата.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chat"></param>
        public void StartAddingWord(string message, Conversation chat)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as AddWordCommand;//ищем и приводим к нужному типу
                                                                                       //т.к. изначально тип IChatCommand
            addingController.AddFirstState(chat);
            command.StartProcessAsync(chat);
        }

        /// <summary>
        /// Выполняет переход к следующей стадии добавления слова в словарь.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chat"></param>
        public void NextStage(string message, Conversation chat)
        {
            var command = Command.Find(x => x is AddWordCommand) as AddWordCommand;

            command.DoForStageAsync(addingController.GetStage(chat), chat, message);

            addingController.NextStage(chat);
        }

        /// <summary>
        /// Позволяет продолжить тренировку.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chat"></param>
        public void ContinueTraining(string message, Conversation chat)
        {
            var command = Command.Find(x => x is TrainingCommand) as TrainingCommand;

            command.NextStepAsync(chat, message);
        }
    }
}