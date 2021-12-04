using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot
{
    /// <summary>
    /// Класс чата.
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// Переменная чата Telegram.Bot.Types.
        /// </summary>
        private Chat telegramChat;
        /// <summary>
        /// Хранит список всех поступивших сообщений чата.
        /// </summary>
        private List<Message> telegramMessages;
        /// <summary>
        /// Библиотека слов (рус/eng/тема) данного чата.
        /// </summary>
        public Dictionary<string, Word> dictionary;
        /// <summary>
        /// Флаг сигнализирующий о том, что идёт процесс добавления слова в словарь.
        /// </summary>
        public bool IsAddingInProcess;
        /// <summary>
        /// Флаг сигнализирующий о том, что идёт тренировка.
        /// </summary>
        public bool IsTraningInProcess;
        /// <summary>
        /// Конструктор. Принимает чат. Инициализирует список всех поступивших сообщений чата и библиотеку слов (рус/eng/тема) данного чата.
        /// </summary>
        /// <param name="chat"></param>
        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
            dictionary = new Dictionary<string, Word>();
        }
        /// <summary>
        /// Добавляет полученное сообщение в список сообщений чата.
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }
        /// <summary>
        /// Добавляет слово в библиотеку слов данного чата.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="word"></param>
        public void AddWord(string key, Word word)
        {
            dictionary.Add(key, word);
        }
        /// <summary>
        /// Удаляет все сообщения из списка сообщений чата.
        /// </summary>
        public void ClearHistory()
        {
            telegramMessages.Clear();
        }
        /// <summary>
        /// Возвращает список всех сообщений чата.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Возвращает ID чата.
        /// </summary>
        /// <returns></returns>
        public long GetId() => telegramChat.Id;
        /// <summary>
        /// Возвращает последнее поступившее сообщение.
        /// </summary>
        /// <returns></returns>
        public string GetLastMessage() => telegramMessages[telegramMessages.Count - 1].Text;
        /// <summary>
        /// Возвращает случайное слово из библиотеки слов для тренировки в звамсимости от направления тренировки.
        /// </summary>
        /// <param name="type">направление тренировки</param>
        /// <returns></returns>
        public string GetTrainingWord(TrainingType type)
        {
            Random rand = new Random();
            int item = rand.Next(0, dictionary.Count);

            try
            {
                Word randomword = dictionary.Values.AsEnumerable().ElementAt(item);

                string text = string.Empty;

                switch (type)
                {
                    case TrainingType.EngToRus:
                        text = randomword.English;
                        break;

                    case TrainingType.RusToEng:
                        text = randomword.Russian;
                        break;
                }

                return text;
            }
            catch(ArgumentNullException)
            {
                return "Словарь пуст";
            }
        }
        /// <summary>
        /// Проверяет соответствие перевода.
        /// </summary>
        /// <param name="type">направление перевода</param>
        /// <param name="word">проверяемое слово</param>
        /// <param name="answer">перевод введённый пользователем</param>
        /// <returns></returns>
        public bool CheckWord(TrainingType type, string word, string answer)
        {
            Word control;

            var result = false;

            switch (type)
            {
                case TrainingType.EngToRus:
                    control = dictionary.Values.FirstOrDefault(x => x.English == word);//ищем проверяемое слово в библиотеке
                    result = control.Russian == answer;//сверяем правильность перевода
                    break;

                case TrainingType.RusToEng:
                    control = dictionary[word];
                    result = control.English == answer;
                    break;
            }

            return result;
        }
    }
}