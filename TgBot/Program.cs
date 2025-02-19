using System.Text;
using OneMistakeLoaderBot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace OneMistakeLoaderBot;

internal class Program
{
    private static async Task Main()
    {
        var botClient = new TelegramBotClient("YOUR_TELEGRAM_BOT_TOKEN");
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery]
        };
        using var cts = new CancellationTokenSource();

        botClient.StartReceiving(UpdateHandler.HandleUpdateAsync, UpdateHandler.HandleErrorAsync, receiverOptions, cts.Token);

        var me = await botClient.GetMe(cts.Token);
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine($"{me.FirstName} started!");
        await Task.Delay(-1, cts.Token);
    }
}
