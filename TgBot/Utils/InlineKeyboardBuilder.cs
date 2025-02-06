using Telegram.Bot.Types.ReplyMarkups;
using YoutubeExplode.Videos.Streams;

namespace OneMistakeLoaderBot.Utils;

public static class InlineKeyboardBuilder
{
    public static readonly Dictionary<string, (IStreamInfo StreamInfo, string Url)> StreamInfoDict = new();

    public static InlineKeyboardMarkup BuildInlineKeyboard(StreamManifest manifest, string url)
    {
        var keyboard = new List<InlineKeyboardButton[]>();
        var i = 0;
        foreach (var stream in manifest.GetAudioOnlyStreams().Where(s => s.Size.MegaBytes < 50))
        {
            var callbackData = $"audio_{i}";
            StreamInfoDict[callbackData] = (stream, url);
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData($"Audio ({stream.Container.Name}) {stream.Size.MegaBytes:N2} MB", callbackData)
            ]);
            i++;
        }
        foreach (var stream in manifest.GetVideoOnlyStreams().Where(s => s.Size.MegaBytes < 50))
        {
            var callbackData = $"video_{i}";
            StreamInfoDict[callbackData] = (stream, url);
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData($"Video ({stream.VideoQuality}) {stream.Size.MegaBytes:N2} MB", callbackData)
            ]);
            i++;
        }
        return new InlineKeyboardMarkup(keyboard);
    }
}