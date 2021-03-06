using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Класс команды выбора поэтов.
    /// </summary>
    public class PoemButtonCommand : AbstractCommand, IKeyBoardCommand
    {
        /// <summary>
        /// Предоставляет интерфейс клиента.
        /// </summary>
        private ITelegramBotClient botClient;
        /// <summary>
        /// Конструктор. Присваивает полю значение команды. Получает объект клиента.
        /// </summary>
        /// <param name="botClient">Объект клиента.</param>
        public PoemButtonCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;

            base.CommandText = "/poembuttons";
        }

        /// <summary>
        /// Выполняет подписку на событие нажатия кнопок клавиатуры.
        /// </summary>
        /// <param name="chat">Чат.</param>
        public void AddCallBack(Conversation chat)
        {
            //this.botClient.OnCallbackQuery -= Bot_Callback;
            this.botClient.OnCallbackQuery += Bot_Callback;
        }

        /// <summary>
        /// Отменяет подписку на событие нажатия кнопок клавиатуры.
        /// </summary>
        private void DelCallBack()
        {
            this.botClient.OnCallbackQuery -= Bot_Callback;
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопок клавиатуры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = "";

            switch (e.CallbackQuery.Data)
            {
                case "pushkin":
                    text = @"Я помню чудное мгновенье:
                                    Передо мной явилась ты,
                                    Как мимолетное виденье,
                                    Как гений чистой красоты."; 
                    break;
                case "esenin":
                    text = @"Не каждый умеет петь,
                                Не каждому дано яблоком
                                Падать к чужим ногам.";
                    break;
                default:
                    break;
            }

            await botClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text);//отправляем в чат сообщение соответствующее выбору
            await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Выбор сделан");//убирает часики на кнопке, позволяет повторное использование кнопок пользователем

            DelCallBack();//чтобы данным экземпляром клавиатуры можно было воспользоваться только один раз
                          //также устраняет проблему, когда вызвана одна клавиатура, а срабатывает CallBack другой клавиатуры
        }

        /// <summary>
        /// Формирует и возвращает клавиатуру.
        /// </summary>
        /// <returns>Объект клавиатуры InlineKeyboardMarkup.</returns>
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

        /// <summary>
        /// Возвращает сообщение поясняющее назначение клавиатуры.
        /// </summary>
        /// <returns>"Выберите поэта"</returns>
        public string InformationalMessage()
        {
            return "Выберите поэта";
        }

        /// <summary>
        /// Проверяет возможность выполнения команды по дополнительным критериям. Здесь наличие слов в словаре конкретного чата.
        /// </summary>
        /// <param name="chat">Чат.</param>
        /// <returns></returns>
        public bool CheckPossibility(Conversation chat)
        {
            return true;
        }
    }
}