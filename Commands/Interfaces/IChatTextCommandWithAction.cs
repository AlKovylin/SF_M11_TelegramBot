namespace TelegramBot.Commands
{
    /// <summary>
    /// Контракт расширяющий IChatTextCommand. Дополнительно предписывает командам, выполняющим конкретное действие,
    /// иметь метод, сообщающий о результате выполнения команды. 
    /// bool DoAction(Conversation chat)
    /// </summary>
    interface IChatTextCommandWithAction : IChatTextCommand
    {
        bool DoAction(Conversation chat);
    }
}
