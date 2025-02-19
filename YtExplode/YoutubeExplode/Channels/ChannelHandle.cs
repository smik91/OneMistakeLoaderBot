﻿using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using YoutubeExplode.Utils.Extensions;

namespace YoutubeExplode.Channels;

/// <summary>
/// Represents a syntactically valid YouTube channel handle.
/// </summary>
public readonly partial struct ChannelHandle(string value)
{
    /// <summary>
    /// Raw handle value.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;
}

public readonly partial struct ChannelHandle
{
    private static bool IsValid(string channelHandle) =>
        channelHandle.All(c => char.IsLetterOrDigit(c) || c is '_' or '-' or '.');

    private static string? TryNormalize(string? channelHandleOrUrl)
    {
        if (string.IsNullOrWhiteSpace(channelHandleOrUrl))
            return null;

        // Check if already passed a handle
        // Tyrrrz
        if (IsValid(channelHandleOrUrl))
            return channelHandleOrUrl;

        // Try to extract the handle from the URL
        // https://www.youtube.com/@Tyrrrz
        var handle = Regex
            .Match(channelHandleOrUrl, @"youtube\..+?/@(.*?)(?:\?|&|/|$)")
            .Groups[1]
            .Value.Pipe(WebUtility.UrlDecode);

        if (!string.IsNullOrWhiteSpace(handle) && IsValid(handle))
            return handle;

        // Invalid input
        return null;
    }

    /// <summary>
    /// Attempts to parse the specified string as a YouTube channel handle or custom URL.
    /// Returns null in case of failure.
    /// </summary>
    public static ChannelHandle? TryParse(string? channelHandleOrUrl) =>
        TryNormalize(channelHandleOrUrl)?.Pipe(handle => new ChannelHandle(handle));

    /// <summary>
    /// Parses the specified string as a YouTube channel handle or custom URL.
    /// </summary>
    public static ChannelHandle Parse(string channelHandleOrUrl) =>
        TryParse(channelHandleOrUrl)
        ?? throw new ArgumentException(
            $"Invalid YouTube channel handle or custom URL '{channelHandleOrUrl}'."
        );

    /// <summary>
    /// Converts string to channel handle.
    /// </summary>
    public static implicit operator ChannelHandle(string channelHandleOrUrl) =>
        Parse(channelHandleOrUrl);

    /// <summary>
    /// Converts channel handle to string.
    /// </summary>
    public static implicit operator string(ChannelHandle channelHandle) => channelHandle.ToString();
}
