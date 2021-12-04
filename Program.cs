// <copyright "Program"	SF_M11_TelegramBot>
// Copyright (c) 2021
// name = "MyFirstBot", username = "StudyMyFirstBot"
// address: t.me/StudyMyFirstBot
// token: "2133806766:AAFrzfC4QZbqEhAn6e-YOZlMUg03UeCPGb0"
// Bot API: https://core.telegram.org/bots/api
// </copyright>

using System;

namespace TelegramBot
{
    class Program
    {
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