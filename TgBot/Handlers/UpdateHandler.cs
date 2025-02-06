using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OneMistakeLoaderBot.Handlers;

public static class UpdateHandler
{
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    if (update.Message != null)
                    {
                        await MessageHandler.HandleMessageAsync(botClient, update.Message, cancellationToken);
                    }

                    break;
                case UpdateType.CallbackQuery:
                    if (update.CallbackQuery != null)
                    {
                        await CallbackQueryHandler.HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
                    }

                    break;
                default:
                    Console.WriteLine($"Unhandled update: {update.Type}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update error: {ex}");
        }
    }

    public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiEx => $"Telegram API Error: [{apiEx.ErrorCode}] {apiEx.Message}",
            _ => exception.ToString()
        };
        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}