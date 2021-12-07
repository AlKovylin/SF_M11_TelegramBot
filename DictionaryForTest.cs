using System.Collections.Generic;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot
{
    class DictionaryForTest
    {
        /// <summary>
        /// Поле для хранения словаря.
        /// </summary>
        public Dictionary<string, Word> dictionary = new Dictionary<string, Word>();

        /// <summary>
        /// Конструктор. Инициализирует словарь.
        /// </summary>
        public DictionaryForTest()
        {
            dictionary.Add("стол", new Word { English = "table", Russian = "стол", Theme = "мебель" });
            dictionary.Add("шкаф", new Word { English = "box", Russian = "шкаф", Theme = "мебель" });
            dictionary.Add("рабочий стол", new Word { English = "desk", Russian = "рабочий стол", Theme = "мебель" });
            dictionary.Add("диван", new Word { English = "sofa", Russian = "диван", Theme = "мебель" });

            dictionary.Add("дрель", new Word { English = "drill", Russian = "дрель", Theme = "инструменты" });
            dictionary.Add("ножовка", new Word { English = "hacksaw", Russian = "ножовка", Theme = "инструменты" });
            dictionary.Add("тиски", new Word { English = "vise", Russian = "тиски", Theme = "инструменты" });
            dictionary.Add("зубило", new Word { English = "chisel", Russian = "зубило", Theme = "инструменты" });

            dictionary.Add("абрикос", new Word { English = "apricot", Russian = "абрикос", Theme = "фрукты" });
            dictionary.Add("лимон", new Word { English = "lemon", Russian = "лимон", Theme = "фрукты" });
            dictionary.Add("яблоко", new Word { English = "apple", Russian = "яблоко", Theme = "фрукты" });
            dictionary.Add("инжир", new Word { English = "fig", Russian = "инжир", Theme = "фрукты" });
        }
    }
}