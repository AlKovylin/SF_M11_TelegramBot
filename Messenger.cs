using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Commands;

namespace TelegramBot
{
    /// <summary>
    /// Класс формирующий ответы в зависимости от полученной команды.
    /// </summary>
    public class Messenger
    {
        /// <summary>
        /// Поле ITelegramBotClient.
        /// </summary>
        private ITelegramBotClient botClient;

        /// <summary>
        /// Экземпляр класса CommandParser.
        /// </summary>
        private CommandParser parser;

        /// <summary>
        /// Конструктор. Принимает ITelegramBotClient. Инициализирует CommandParser и запускает функцию регистрации команд.
        /// </summary>
        /// <param name="botClient"></param>
        public Messenger(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            parser = new CommandParser();

            RegisterCommands();
        }

        /// <summary>
        /// Добавляет объекты команд в список команд экземпляра анализатора команд CommandParser.
        /// </summary>
        private void RegisterCommands()
        {
            parser.AddCommand(new SayHiCommand());
            parser.AddCommand(new PoemButtonCommand(botClient));
            parser.AddCommand(new AddWordCommand(botClient));
            parser.AddCommand(new DeleteWordCommand());
            parser.AddCommand(new TrainingCommand(botClient));
            parser.AddCommand(new StopTrainingCommand());
            parser.AddCommand(new ShowDictionaryCommand(botClient));
        }

        /// <summary>
        /// Сортировщик входящих сообщений. Проверяет не происходит ли в данный момент выполнение каких-то последовательностей требующих продолжения выполнения
        /// и либо запускает следующие итерации, либо передаёт управление функции обеспечивающей запуск выполнения полученной команды, либо отправляет в чат сообщение
        /// о не известной команде.
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public async Task MakeAnswer(Conversation chat)
        {
            var lastmessage = chat.GetLastMessage();//получаем последнее поступившее сообщение

            if (chat.IsTraningInProcess && !parser.IsTextCommand(lastmessage))//если запущен процесс тренировки и полученное сообщение
            {                                                                 //не является командой реализующей интерфейс IsTextCommand
                parser.ContinueTraining(lastmessage, chat);//продолжаем тренировку

                return;
            }

            if (chat.IsAddingInProcess)//если активен процесс добавления нового слова в словарь
            {
                parser.NextStage(lastmessage, chat);//вызов функции движения по стадиям ввода

                return;
            }

            if (parser.IsMessageCommand(lastmessage))//если получена команда из списка
            {
                await ExecCommand(chat, lastmessage);//переходим в функцию перебора команд
            }
            else//если полученное сообщение не соответствует ни одному из условий выше
            {
                var text = CreateTextMessage();//получаем текст сообщения о не зарегистрированной команде

                await SendText(chat, text);//и передаём его в чат
            }
        }

        /// <summary>
        /// Обеспечивает выполнение действий в соответствии с командой из чата.
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task ExecCommand(Conversation chat, string command)
        {
            if (parser.IsTextCommand(command))
            {
                var text = parser.GetMessageText(command, chat);

                await SendText(chat, text);
            }

            if (parser.IsButtonCommand(command))
            {
                if(parser.IsButtonCommandCheck(command))//если реализует интерфейс IKeyBoardCommandCheck
                {
                    if (parser.ButtonCommandCheckPossibility(command, chat))//если нет препятствий для выполнения
                    {
                        await CreatKeyboard(chat, command);//формируем и отправляем в чат клавиатуру
                    }
                    else
                    {
                        await SendText(chat, parser.GetErrCheckText(command));//отправляем сообщение о невозможности выполнения
                    }
                }
                else//если не реализует IKeyBoardCommandCheck, то доп. проверка не нужна
                {
                    await CreatKeyboard(chat, command);
                }
            }

            if (parser.IsAddingCommand(command))
            {
                chat.IsAddingInProcess = true;
                parser.StartAddingWord(command, chat);
            }
        }

        /// <summary>
        /// Возврашает сообщение о не зарегистрированной команде
        /// </summary>
        /// <returns>"Not a command"</returns>
        private string CreateTextMessage()
        {
            var text = "Not a command";

            return text;
        }

        /// <summary>
        /// Передаёт в чат текстовое сообщение.
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private async Task SendText(Conversation chat, string text)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }

        /// <summary>
        /// Передаёт в чат клавиатуру.
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="text"></param>
        /// <param name="keyboard"></param>
        /// <returns></returns>
        private async Task SendTextWithKeyBoard(Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }

        /// <summary>
        /// Вызывает процедуры: формирование клавиатуры, поясняющего её назначение сообщения, 
        /// подписки на событие нажатия кнопок, отправку в чат.
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task CreatKeyboard(Conversation chat, string command)
        {
            var keys = parser.GetKeyBoard(command);
            var text = parser.GetInformationalMeggase(command);
            parser.AddCallback(command, chat);

            await SendTextWithKeyBoard(chat, text, keys);
        }
    }
}