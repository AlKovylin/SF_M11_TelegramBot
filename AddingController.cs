using System.Collections.Generic;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot
{
    /// <summary>
    /// Класс контроллер состояния чата.
    /// </summary>
    public class AddingController
    {
        /// <summary>
        /// Поле для хранения ID чата и текущей стадии добавления слова.
        /// </summary>
        private Dictionary<long, AddingState> ChatAdding;

        /// <summary>
        /// Конструктор. Создаёт экземпляр библиотеки для хранения ID чата и стадии.
        /// </summary>
        public AddingController()
        {
            ChatAdding = new Dictionary<long, AddingState>();
        }

        /// <summary>
        /// Обозначает начало процесса добавления слова, записывая в поле для хранения ID чата и текущую стадию (первую).
        /// </summary>
        /// <param name="chat"></param>
        public void AddFirstState(Conversation chat)
        {
            ChatAdding.Add(chat.GetId(), AddingState.Russian);
        }

        /// <summary>
        /// Выполняет переход к следующей стадии. При достижении стадии "Finish" очищает поле для хранения текущей стадии.
        /// </summary>
        /// <param name="chat"></param>
        public void NextStage(Conversation chat)
        {
            var currentstate = ChatAdding[chat.GetId()];
            ChatAdding[chat.GetId()] = currentstate + 1;

            if (ChatAdding[chat.GetId()] == AddingState.Finish)
            {
                chat.IsAddingInProcess = false;
                ChatAdding.Remove(chat.GetId());
            }
        }

        /// <summary>
        /// Возвращает текущую стадию добавления слова.
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public AddingState GetStage(Conversation chat)
        {
            return ChatAdding[chat.GetId()];
        }
    }
}