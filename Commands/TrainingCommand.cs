using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Класс команды для проведения тренировки
    /// </summary>
    public class TrainingCommand : AbstractCommand, IKeyBoardCommand, ICommandCheck
    {
        /// <summary>
        /// Поле клиента.
        /// </summary>
        private ITelegramBotClient botClient;
        /// <summary>
        /// Поле для хранения: ID клиента/тип тренировки.
        /// </summary>
        private Dictionary<long, TrainingType> training;
        /// <summary>
        /// Поле для хранения: ID клиента/чат.
        /// </summary>
        private Dictionary<long, Conversation> trainingChats;
        /// <summary>
        /// Поле для хранения: ID клиента/текущее слово.
        /// </summary>
        private Dictionary<long, string> activeWord;
        /// <summary>
        /// Конструктор команды "Тренировка".
        /// </summary>
        /// <param name="botClient"></param>
        public TrainingCommand(ITelegramBotClient botClient)
        {
            CommandText = "/training";

            this.botClient = botClient;

            training = new Dictionary<long, TrainingType>();
            trainingChats = new Dictionary<long, Conversation>();
            activeWord = new Dictionary<long, string>();
        }
        /// <summary>
        /// Возвращает клавиатуру выбора направления тренировки.
        /// </summary>
        /// <returns></returns>
        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "С русского на английский",
                    CallbackData = "rustoeng"
                },

                new InlineKeyboardButton
                {
                    Text = "С английского на русский",
                    CallbackData = "engtorus"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }
        /// <summary>
        /// Возвращает информационное сообщение.
        /// </summary>
        /// <returns>"Выберите тип тренировки. Для окончания тренировки введите команду /stop"</returns>
        public string InformationalMessage()
        {
            return "Выберите тип тренировки. Для окончания тренировки введите команду /stop";
        }
        /// <summary>
        /// Выполняет подписку на событие нажатия кнопок клавиатуры Тренировка.
        /// </summary>
        /// <param name="chat"></param>
        public void AddCallBack(Conversation chat)
        {
            trainingChats.Add(chat.GetId(), chat);

            this.botClient.OnCallbackQuery += Bot_Callback;
        }
        /// <summary>
        /// Отменяет подписку на событие нажатия кнопок клавиатуры Тренировка.
        /// </summary>
        private void DelCallBack()
        {
            this.botClient.OnCallbackQuery -= Bot_Callback;
        }
        /// <summary>
        /// Обработка события нажатия кнопок клавиатуры Тренировка.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = "";

            var id = e.CallbackQuery.Message.Chat.Id;

            var chat = trainingChats[id];

            switch (e.CallbackQuery.Data)
            {
                case "rustoeng":
                    ResetTrainingDirection(id);

                    training.Add(id, TrainingType.RusToEng);

                    text = chat.GetTrainingWord(TrainingType.RusToEng);

                    break;
                case "engtorus":
                    ResetTrainingDirection(id);

                    training.Add(id, TrainingType.EngToRus);

                    text = chat.GetTrainingWord(TrainingType.EngToRus);
                    break;
                default:
                    break;
            }

            chat.IsTraningInProcess = true;

            activeWord.Clear();//устраняет ошибку в следующей строке возникающую при рандомном выборе того-же слова, что и в предыдущий раз
                               //устраняет ошибку при проверке правильности введённого слова после выбора другого нарпавления тренировки
                               //при повторном запуске тренировки

            activeWord.Add(id, text);

            if (trainingChats.ContainsKey(id))
            {
                trainingChats.Remove(id);
            }

            await botClient.SendTextMessageAsync(id, text);
            await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);

            DelCallBack();//чтобы данным экземпляром клавиатуры можно было воспользоваться только один раз
                          //также устраняет проблему, когда вызвана одна клавиатура, а срабатывает CallBack другой клавиатуры
        }
        /// <summary>
        /// Производит проверку корректности перевода, формирует ответ, отправляет в чат следующее слово.
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="message"></param>
        public async void NextStepAsync(Conversation chat, string message)
        {
            var type = training[chat.GetId()];
            var word = activeWord[chat.GetId()];

            var check = chat.CheckWord(type, word, message);

            var text = "";

            if (check)
            {
                text = "Правильно!";
            }
            else
            {
                text = "Неправильно!";
            }

            text = text + " Следующее слово: ";

            var newword = chat.GetTrainingWord(type);

            text = text + newword;

            activeWord[chat.GetId()] = newword;


            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }
        /// <summary>
        /// Проверяет возможность выполнения команды по дополнительным критериям. Здесь наличие слов в словаре конкретного чата.
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public bool CheckPossibility(Conversation chat)
        {
            return chat.CheckDictionary();
        }
        /// <summary>
        /// Возвращает сообщение если использование клавиатуры не возможно.
        /// </summary>
        /// <returns></returns>
        public string ReturnErrText()
        {
            return "Словарь пуст. Начните с добавления слов.";
        }
        /// <summary>
        /// Удаляет запись о направлении тренировки для конкретного чата. Иначе возникает ошибка поиска слова для проверки
        /// при повторном начале тренировки.
        /// </summary>
        /// <param name="id"></param>
        private void ResetTrainingDirection(long id)
        {
            if (training.ContainsKey(id))
            {
                training.Remove(id);
            }
        }
    }
}