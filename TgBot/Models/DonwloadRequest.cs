using Telegram.Bot;
using YoutubeExplode.Videos.Streams;

namespace OneMistakeLoaderBot.Models;

public class DownloadRequest
{
    public ITelegramBotClient BotClient { get; }
    public long ChatId { get; }
    public IStreamInfo StreamInfo { get; }
    public string Url { get; }
    public CancellationToken CancellationToken { get; }

    public DownloadRequest(ITelegramBotClient botClient,
        long chatId,
        IStreamInfo streamInfo,
        string url,
        CancellationToken cancellationToken)
    {
        BotClient = botClient;
        ChatId = chatId;
        StreamInfo = streamInfo;
        Url = url;
        CancellationToken = cancellationToken;
    }
}