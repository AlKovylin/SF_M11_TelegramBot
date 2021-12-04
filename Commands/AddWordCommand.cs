using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Класс команды добавления слова в словарь.
    /// </summary>
    public class AddWordCommand : AbstractCommand
    {
        /// <summary>
        /// Поле ITelegramBotClient интерфейса клиента.
        /// </summary>
        private ITelegramBotClient botClient;
        /// <summary>
        /// Поле буфер для хранения ID клиента и вводимого слова.
        /// </summary>
        private Dictionary<long, Word> Buffer;
        /// <summary>
        /// Конструктор. Присваивает полю значение команды. Принимает интерфейс клиента. Создаёт экземпляр буфера.
        /// </summary>
        /// <param name="botClient">Интерфейс клиента.</param>
        public AddWordCommand(ITelegramBotClient botClient)
        {
            CommandText = "/addword";

            this.botClient = botClient;

            Buffer = new Dictionary<long, Word>();
        }
        /// <summary>
        /// Начинает процедуру ввода нового слова.
        /// </summary>
        /// <param name="chat"></param>
        public async void StartProcessAsync(Conversation chat)
        {
            Buffer.Add(chat.GetId(), new Word());

            var text = "Введите русское значение слова";

            await SendCommandText(text, chat.GetId());//отправляем сообщение в чат клиента
        }
        /// <summary>
        /// Формирует запрос на ввод слова в соответствии со стадией переключаемой контроллером. 
        /// Добавляет новое слово в словарь конкретного чата.
        /// </summary>
        /// <param name="addingState">Текущая стадия.</param>
        /// <param name="chat">Чат.</param>
        /// <param name="message">Сообщение введённое пользователем.</param>
        public async void DoForStageAsync(AddingState addingState, Conversation chat, string message)
        {
            var word = Buffer[chat.GetId()];
            var text = "";

            switch (addingState)
            {
                case AddingState.Russian:
                    word.Russian = message;

                    text = "Введите английское значение слова";
                    break;

                case AddingState.English:
                    word.English = message;

                    text = "Введите тематику";
                    break;

                case AddingState.Theme:
                    word.Theme = message;

                    text = "Успешно! Слово " + word.English + " добавлено в словарь. ";

                    chat.dictionary.Add(word.Russian, word);//добавляем слово в словарь, ключ - значение на русском

                    Buffer.Remove(chat.GetId());//очищаем буфер
                    break;
            }

            await SendCommandText(text, chat.GetId());
        }
        /// <summary>
        /// Отправляет сообщение в чат.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        private async Task SendCommandText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chatId: chat, text: text);
        }
    }
}