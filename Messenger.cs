using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace SF_M11_TelegramBot
{
    class Messenger
    {
        /*public string CreateTextMessage(Conversation chat)
        {
            var delimiter = ",";
            var text = "Your history: " + string.Join(delimiter, chat.GetTextMessages().ToArray());
            return text;
        }*/

        public string CreateTextMessage(Conversation chat)
        {
            var text = "";

            switch (chat.GetLastMessage())
            {
                case "/saymehi":
                    text = "Привет";
                    break;
                case "/askme":
                    text = "Как дела?";
                    break;
                case "/poembuttons":
                    text = "Выберите поэта";
                    break;
                default:
                    var delimiter = ",";
                    text = "История ваших сообщений: " + string.Join(delimiter, chat.GetTextMessages().ToArray());
                    break;
            }
            return text;
        }

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                  Text = "Пушкин",
                  CallbackData = "pushkin"
                },

                new InlineKeyboardButton
                {
                  Text = "Есенин",
                  CallbackData = "esenin"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }
    }
}
