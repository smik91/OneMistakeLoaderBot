using OneMistakeLoaderBot.Models;
using OneMistakeLoaderBot.Services;
using OneMistakeLoaderBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OneMistakeLoaderBot.Handlers;

public static class CallbackQueryHandler
{
    public static async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery?.Message == null)
        {
            return;
        }
        
        var chatId = callbackQuery.Message.Chat.Id;
        var callbackData = callbackQuery.Data;
        if (InlineKeyboardBuilder.StreamInfoDict.TryGetValue(callbackData, out var info))
        {
            var request = new DownloadRequest(botClient, chatId, info.StreamInfo, info.Url, cancellationToken);
            Task.Run(() => DownloadManager.Instance.EnqueueDownload(request), cancellationToken);
            await botClient.SendMessage(chatId, "Video queued for download...", cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendMessage(chatId, "Error: Invalid download data.", cancellationToken: cancellationToken);
        }
    }
}