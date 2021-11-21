using Telegram.Bot;
using Telegram.Bot.Args;

namespace SF_M11_TelegramBot
{
    class BotWorker
    {
        static private ITelegramBotClient botClient;
        static private BotMessageLogic logic;

        public void Inizalize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic(botClient);
        }

        public void Start()
        {
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        public void Stop()
        {
            botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                await logic.Response(e);
            }
        }
    }
}
