namespace TelegramBot.EnglishTrainer.Model
{
    /// <summary>
    /// Класс описывающий структуру слова для словаря.
    /// </summary>
    public class Word
    {
        public string English { get; set; }
        public string Russian { get; set; }
        public string Theme { get; set; }
    }
}