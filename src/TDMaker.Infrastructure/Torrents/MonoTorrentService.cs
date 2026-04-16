namespace TDMaker.Infrastructure.Torrents;

using MonoTorrent;
using Microsoft.Extensions.Logging;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;
using TDMaker.Infrastructure.Support;

public sealed partial class MonoTorrentService(
    ILogger<MonoTorrentService> logger) : ITorrentService
{
    public async Task<IReadOnlyList<string>> CreateAsync(
        MediaInspectionResult inspection,
        ReleaseProfile profile,
        string outputDirectory,
        CancellationToken cancellationToken = default)
    {
        var trackers = profile.Trackers
            .Where(x => x.Enabled && !string.IsNullOrWhiteSpace(x.AnnounceUrl))
            .ToArray();

        if (trackers.Length == 0)
        {
            return [];
        }

        var created = new List<string>(trackers.Length);
        var fileSource = new PathTorrentFileSource(inspection.Inputs, inspection.OutputName);

        foreach (var tracker in trackers)
        {
            var creator = new TorrentCreator
            {
                Announce = tracker.AnnounceUrl,
                Comment = inspection.Title,
                CreatedBy = "TDMaker",
                Private = true,
                Publisher = "TDMaker",
                PublisherUrl = "https://github.com/McoreD/TDMaker",
                StoreMD5 = false
            };

            if (profile.TorrentPieceLengthKiB is > 0)
            {
                creator.PieceLength = profile.TorrentPieceLengthKiB.Value * 1024;
            }

            var trackerHost = StringUtilities.SanitizeFileName(new Uri(tracker.AnnounceUrl).Host);
            var trackerDirectory = Path.Combine(outputDirectory, "torrents", trackerHost);
            Directory.CreateDirectory(trackerDirectory);

            var torrentPath = Path.Combine(trackerDirectory, $"{inspection.OutputName}.torrent");
            await creator.CreateAsync(fileSource, torrentPath, cancellationToken);
            created.Add(torrentPath);
        }

        LogTorrentFilesCreated(logger, created.Count);
        return created;
    }

    private sealed class PathTorrentFileSource(IReadOnlyList<string> inputs, string torrentName) : ITorrentFileSource
    {
        public IEnumerable<FileMapping> Files => EnumerateFiles();
        public string TorrentName => torrentName;

        private IEnumerable<FileMapping> EnumerateFiles()
        {
            foreach (var input in inputs)
            {
                if (File.Exists(input))
                {
                    var info = new FileInfo(input);
                    yield return new FileMapping(info.FullName, info.Name, info.Length);
                    continue;
                }

                if (!Directory.Exists(input))
                {
                    continue;
                }

                foreach (var file in Directory.EnumerateFiles(input, "*.*", SearchOption.AllDirectories))
                {
                    var info = new FileInfo(file);
                    var relativePath = Path.GetRelativePath(input, file);
                    yield return new FileMapping(info.FullName, relativePath, info.Length);
                }
            }
        }
    }

    [LoggerMessage(EventId = 6001, Level = LogLevel.Information, Message = "Created {Count} torrent files")]
    private static partial void LogTorrentFilesCreated(ILogger logger, int count);
}
