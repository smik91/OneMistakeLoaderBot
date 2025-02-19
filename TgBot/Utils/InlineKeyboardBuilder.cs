using Telegram.Bot.Types.ReplyMarkups;
using YoutubeExplode.Videos.Streams;

namespace OneMistakeLoaderBot.Utils;

public static class InlineKeyboardBuilder
{
    public static readonly Dictionary<string, (IStreamInfo StreamInfo, string Url)> StreamInfoDict = new();

    public static InlineKeyboardMarkup BuildInlineKeyboard(StreamManifest manifest, string url)
    {
        var keyboard = new List<InlineKeyboardButton[]>();
        var videoStreams = manifest.GetVideoOnlyStreams()
            .Where(s => s.Size.MegaBytes < 50)
            .ToList();
        
        for (int i = 0; i < videoStreams.Count; i+=2)
        {
            var buttons = new List<InlineKeyboardButton>(2);
            
            var firstCallbackData = $"video_{i}";
            var firstStream = videoStreams[i];
            StreamInfoDict[firstCallbackData] = (firstStream, url);
            buttons.Add(InlineKeyboardButton.WithCallbackData($"Video ({firstStream.VideoQuality}) {firstStream.Size.MegaBytes:N2} MB", firstCallbackData));
            
            if (i + 1 < videoStreams.Count)
            {
                var secondStream = videoStreams[i+1];
                var secondCallbackData = $"video_{i + 1}";
                StreamInfoDict[secondCallbackData] = (secondStream, url);
                buttons.Add(InlineKeyboardButton.WithCallbackData($"Video ({secondStream.VideoQuality}) {secondStream.Size.MegaBytes:N2} MB", secondCallbackData));
            }
            
            keyboard.Add(buttons.ToArray());
        }
        
        return new InlineKeyboardMarkup(keyboard);
    }
}