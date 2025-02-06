using OneMistakeLoaderBot.Services;
using OneMistakeLoaderBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OneMistakeLoaderBot.Handlers;

public static class MessageHandler
{
    private const string StartMessage = "Hello,\nSend YouTube video links for download (max 30 mins).";

    public static async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        Console.WriteLine($"{message.From?.FirstName} ({message.From?.Id}): {message.Text}");

        if (message.Text == "/start")
        {
            await botClient.SendMessage(message.Chat.Id,
                StartMessage,
                cancellationToken: cancellationToken);
        }
        else if (string.IsNullOrEmpty(message.Text) || !UrlValidator.IsYouTubeUrl(message.Text))
        {
            await botClient.SendMessage(message.Chat.Id, "Invalid reference!", cancellationToken: cancellationToken);
        }
        else
        {
            var manifest = await DownloadManager.Instance.GetManifestAsync(message.Text);
            var keyboard = InlineKeyboardBuilder.BuildInlineKeyboard(manifest, message.Text);
            await botClient.SendMessage(message.Chat.Id, "Choose format:", replyMarkup: keyboard, cancellationToken: cancellationToken);
        }
    }
}