// <copyright "Program"	SF_M11_TelegramBot>
// Copyright (c) 2021
// name = "TrainingBot", username = "@StudyMyFirstBot"
// address: t.me/BotForEnglishLanguageTraining
// token: "2133806766:AAFrzfC4QZbqEhAn6e-YOZlMUg03UeCPGb0"
// Bot API: https://core.telegram.org/bots/api
// </copyright>

using System;

namespace TelegramBot
{
    class Program
    {
        /// <summary>
        /// Точка входа.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var bot = new BotWorker();

            bot.Inizalize();
            bot.Start();

            Console.WriteLine("Напишите stop для прекращения работы");

            string command;
            do
            {
                command = Console.ReadLine();

            } while (command != "stop");

            bot.Stop();
        }
    }
}