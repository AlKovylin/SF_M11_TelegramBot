using System.Collections.Generic;
using Telegram.Bot.Types;

namespace SF_M11_TelegramBot
{
    class Conversation//Разговор
    {
        private Chat telegramChat;
        private List<Message> telegramMessages;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        public void ClearHistory()
        {
            telegramMessages.Clear();
        }

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach (var message in telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }
            return textMessages;
        }

        public long GetId() => telegramChat.Id;
    }
}
