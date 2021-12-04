using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    /// <summary>
    /// Класс основных команд бота.
    /// </summary>
   public class BotWorker
    {
        /// <summary>
        /// Поле ITelegramBotClient.
        /// </summary>
        private ITelegramBotClient botClient;
        /// <summary>
        /// Поле BotMessageLogic.
        /// </summary>
        private BotMessageLogic logic;
        /// <summary>
        /// Создаёт объекты TelegramBotClient и BotMessageLogic.
        /// </summary>
        public void Inizalize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic(botClient);
        }
        /// <summary>
        /// Запускает бот. Выполняет подписку на событие получения сообщения.
        /// </summary>
        public void Start()
        {
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }
        /// <summary>
        /// Завершает работу бота.
        /// </summary>
        public void Stop()
        {
            botClient.StopReceiving();
        }
        /// <summary>
        /// Запускает асинхронную процедуру обработки полученного сообщения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null)
            {
                await logic.Response(e);
            }
        }
    }
}