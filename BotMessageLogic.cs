using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    /// <summary>
    /// Класс логики работы бота.
    /// </summary>
    public class BotMessageLogic
    {
        /// <summary>
        /// Поле класса формирующего диалог сообщения.
        /// </summary>
        private Messenger messanger;
        /// <summary>
        /// Поле/библотека для хранения списка чатов.
        /// </summary>
        private Dictionary<long, Conversation> chatList;
        /// <summary>
        /// Поле ITelegramBotClient.
        /// </summary>
        private ITelegramBotClient botClient;
        /// <summary>
        /// Конструктор. Принимает ITelegramBotClient. Инициализирует экземпляр Messenger и поле/библотеку для хранения списка чатов.
        /// </summary>
        /// <param name="botClient"></param>
        public BotMessageLogic(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            messanger = new Messenger(botClient);
            chatList = new Dictionary<long, Conversation>();
        }
        /// <summary>
        /// Запускает процедуру обработки входящего сообщения по ID чата. Новый чат добавляет в список чатов.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task Response(MessageEventArgs e)
        {
            var Id = e.Message.Chat.Id;//получаем Id чата

            if (!chatList.ContainsKey(Id))//если такого чата нет в списке,
            {
                var newchat = new Conversation(e.Message.Chat);//то создаём новый чат

                chatList.Add(Id, newchat);//и добавляем его в список чатов
            }

            var chat = chatList[Id];//получаем чат из списка

            chat.AddMessage(e.Message);

            await SendMessage(chat);
        }
        /// <summary>
        /// Запускает процедуру обработки полученного сообщения, формирования и отправки ответного сообщения.
        /// </summary>
        /// <param name="chat">Конкретный чат.</param>
        /// <returns></returns>
        private async Task SendMessage(Conversation chat)
        {
            await messanger.MakeAnswer(chat);            
        }
    }
}