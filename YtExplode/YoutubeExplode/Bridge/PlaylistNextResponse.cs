﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Lazy;
using YoutubeExplode.Utils;
using YoutubeExplode.Utils.Extensions;

namespace YoutubeExplode.Bridge;

internal partial class PlaylistNextResponse(JsonElement content) : IPlaylistData
{
    [Lazy]
    private JsonElement? ContentRoot =>
        content
            .GetPropertyOrNull("contents")
            ?.GetPropertyOrNull("twoColumnWatchNextResults")
            ?.GetPropertyOrNull("playlist")
            ?.GetPropertyOrNull("playlist");

    [Lazy]
    public bool IsAvailable => ContentRoot is not null;

    [Lazy]
    public string? Title => ContentRoot?.GetPropertyOrNull("title")?.GetStringOrNull();

    [Lazy]
    public string? Author =>
        ContentRoot
            ?.GetPropertyOrNull("ownerName")
            ?.GetPropertyOrNull("simpleText")
            ?.GetStringOrNull();

    public string? ChannelId => null;

    public string? Description => null;

    [Lazy]
    public int? Count =>
        ContentRoot
            ?.GetPropertyOrNull("totalVideosText")
            ?.GetPropertyOrNull("runs")
            ?.EnumerateArrayOrNull()
            ?.FirstOrNull()
            ?.GetPropertyOrNull("text")
            ?.GetStringOrNull()
            ?.ParseIntOrNull()
        ?? ContentRoot
            ?.GetPropertyOrNull("videoCountText")
            ?.GetPropertyOrNull("runs")
            ?.EnumerateArrayOrNull()
            ?.ElementAtOrNull(2)
            ?.GetPropertyOrNull("text")
            ?.GetStringOrNull()
            ?.ParseIntOrNull();

    [Lazy]
    public IReadOnlyList<ThumbnailData> Thumbnails => Videos.FirstOrDefault()?.Thumbnails ?? [];

    [Lazy]
    public IReadOnlyList<PlaylistVideoData> Videos =>
        ContentRoot
            ?.GetPropertyOrNull("contents")
            ?.EnumerateArrayOrNull()
            ?.Select(j => j.GetPropertyOrNull("playlistPanelVideoRenderer"))
            .WhereNotNull()
            .Select(j => new PlaylistVideoData(j))
            .ToArray() ?? [];

    [Lazy]
    public string? VisitorData =>
        content
            .GetPropertyOrNull("responseContext")
            ?.GetPropertyOrNull("visitorData")
            ?.GetStringOrNull();
}

internal partial class PlaylistNextResponse
{
    public static PlaylistNextResponse Parse(string raw) => new(Json.Parse(raw));
}
