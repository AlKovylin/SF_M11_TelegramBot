using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Класс команды возвращающей в чат словарь конкретного чата.
    /// </summary>
    public class ShowDictionaryCommand : AbstractCommand, IKeyBoardCommand, ICommandCheck
    {
        /// <summary>
        /// Предоставляет интерфейс клиента.
        /// </summary>
        ITelegramBotClient botClient;
        /// <summary>
        /// Поле чата.
        /// </summary>
        Conversation chat;
        /// <summary>
        /// Конструктор. Присваивает полю значение команды. Получает объект клиента.
        /// </summary>
        /// <param name="botClient">Объект клиента.</param>
        public ShowDictionaryCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;

            CommandText = "/dictionary";
        }
        /// <summary>
        /// Возвращает объект клавиатуры.
        /// </summary>
        /// <returns></returns>
        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "По алфавиту",
                    CallbackData = "abc"
                },

                new InlineKeyboardButton
                {
                    Text = "По темам",
                    CallbackData = "tema"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }
        /// <summary>
        /// Выполняет подписку на событие нажатия кнопок клавиатуры.
        /// </summary>
        /// <param name="chat"></param>
        public void AddCallBack(Conversation chat)
        {
            this.chat = chat;
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
        /// Обрабатывает событие нажатия кнопок клавиатуры Поэты.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            Dictionary<string, Word> dictionary = null;
            var text = "";

            switch (e.CallbackQuery.Data)
            {
                case "abc":
                    dictionary = DictionarySortABC(chat);
                    break;
                case "tema":
                    dictionary = DictionarySortTema(chat);
                    break;
                default:
                    break;
            }

            foreach (var word in dictionary)
            {
                text = word.Value.Russian + ", " + word.Value.English + ", " + word.Value.Theme;
                await botClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text);//отправляем в чат сообщение соответствующее выбору
            }

            await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);//убирает часики на кнопке, позволяет повторное использование кнопок пользователем

            DelCallBack();//чтобы данным экземпляром клавиатуры можно было воспользоваться только один раз
                          //также устраняет проблему, когда вызвана одна клавиатура, а срабатывает CallBack другой клавиатуры
        }
        /// <summary>
        /// Возвращает информационное сообщение - пояснение к клавиатуре.
        /// </summary>
        /// <returns></returns>
        public string InformationalMessage()
        {
            return "Выберите тип сортировки словаря.";
        }
        /// <summary>
        /// Проверяет возможность выполнения команды
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public bool CheckPossibility(Conversation chat)
        {
            return chat.CheckDictionary();
        }
        /// <summary>
        /// Возвращает сообщение о причине невозможности выполнения.
        /// </summary>
        /// <returns></returns>
        public string ReturnErrText()
        {
            return "Словарь пуст. Начните с добавления слов.";
        }
        /// <summary>
        /// Возвращает словарь отсортированный по алфавиту.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Word> DictionarySortABC(Conversation chat)
        {
            ////////для тестирования из тестового словаря раскамментить///////
            ///при запуске бота нужно добавить хотя бы одно слово в словарь///
            //DictionaryForTest test = new DictionaryForTest();
            //Dictionary<string, Word> Temp = test.dictionary;
            ///////////////////////////////////////////////////

            ///////а эту закамментить///////
            Dictionary<string, Word> Temp = chat.GetDictionary();

            Dictionary<string, Word> dictionary = new Dictionary<string, Word>();

            foreach (var word in Temp.OrderBy(i => i.Key))
            {
                dictionary.Add(word.Key, word.Value);
            }

            return dictionary;
        }
        /// <summary>
        /// Возвращает словарь отсортированный по темам.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Word> DictionarySortTema(Conversation chat)
        {
            ////////для тестирования из тестового словаря раскамментить///////
            //////при запуске бота нужно добавить хотя бы одно слово в словарь///
            //DictionaryForTest test = new DictionaryForTest();
            //Dictionary<string, Word> Temp = test.dictionary;
            ///////////////////////////////////////////////////
            
            ///////а эту закамментить///////
            Dictionary<string, Word> Temp = chat.GetDictionary();

            Dictionary<string, Word> dictionary = new Dictionary<string, Word>();

            foreach (var word in Temp.OrderBy(i => i.Value.Theme))
            {
                dictionary.Add(word.Key, word.Value);
            }

            return dictionary;
        }
    }
}