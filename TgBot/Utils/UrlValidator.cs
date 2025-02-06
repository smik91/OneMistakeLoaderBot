using System.Text.RegularExpressions;

namespace OneMistakeLoaderBot.Utils;

public static class UrlValidator
{
    public static bool IsYouTubeUrl(string url)
    {
        const string pattern = @"^(https?://)?(www\.)?(youtube\.com|youtu\.be)/(watch\?v=|embed/|v/|.+\?v=)?([^&=%\?]{11})";
        var isMatch = new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(url);
        return isMatch;
    }
}