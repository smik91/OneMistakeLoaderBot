using System.Collections.Concurrent;
using OneMistakeLoaderBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace OneMistakeLoaderBot.Services;

public class DownloadManager
{
    private readonly YoutubeClient _youtubeClient = new();
    private readonly ConcurrentQueue<DownloadRequest> _queue = new();
    private readonly SemaphoreSlim _semaphore = new(5);
    private const string Dir = "VideoCache";
    private static readonly Lazy<DownloadManager> _instance = new(() => new DownloadManager());
    public static DownloadManager Instance => _instance.Value;

    private DownloadManager() { }

    public async Task EnqueueDownload(DownloadRequest request)
    {
        _queue.Enqueue(request);
        await ProcessQueue();
    }

    private async Task ProcessQueue()
    {
        await _semaphore.WaitAsync();
        try
        {
            while (_queue.TryDequeue(out var request))
            {
                var streamInfo = request.StreamInfo;
                var video = await _youtubeClient.Videos.GetAsync(request.Url);
                var fileName = GenerateFileName(video.Title);
                Directory.CreateDirectory(Dir);
                var path = Path.Combine(Dir, $"{fileName}.{streamInfo.Container.Name}");
                await _youtubeClient.Videos.Streams.DownloadAsync(streamInfo, path);
                await using var stream = File.OpenRead(path);
                await request.BotClient.SendVideo(request.ChatId,
                    InputFile.FromStream(stream),
                    caption: $"{video.Title}",
                    cancellationToken: request.CancellationToken);
                stream.Close();
                File.Delete(path);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private static string GenerateFileName(string title)
    {
        var newTitle = new string(title.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
        return newTitle;
    }

    public async Task<StreamManifest> GetManifestAsync(string url)
    {
        var manifest = await _youtubeClient.Videos.Streams.GetManifestAsync(url);
        return manifest;
    }
}