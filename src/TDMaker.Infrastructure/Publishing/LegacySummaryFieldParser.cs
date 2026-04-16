namespace TDMaker.Infrastructure.Publishing;

using System.Text.RegularExpressions;

internal sealed class LegacySummaryFieldParser
{
    private readonly Dictionary<string, string> _general = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _video = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Dictionary<string, string>> _audio = [];

    public LegacySummaryFieldParser(string summary)
    {
        var current = string.Empty;

        foreach (var rawLine in summary.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries))
        {
            var line = rawLine.TrimEnd();
            var separatorIndex = line.IndexOf(" : ", StringComparison.Ordinal);

            if (separatorIndex < 0)
            {
                current = line.Split(" #", StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                if (string.Equals(current, "Audio", StringComparison.OrdinalIgnoreCase) && _audio.Count == 0)
                {
                    _audio.Add(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
                }

                continue;
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 3)..].Trim();
            var token = $"%{current}_{SanitizeKey(key)}%";

            switch (current)
            {
                case "General":
                    _general[token] = value;
                    break;
                case "Video":
                    _video[token] = value;
                    break;
                case "Audio":
                    if (_audio.Count == 0)
                    {
                        _audio.Add(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
                    }

                    _audio[0][token] = value;
                    break;
            }
        }
    }

    public string ReplaceGeneral(string template) => Replace(template, _general);
    public string ReplaceVideo(string template) => Replace(template, _video);

    public string ReplaceAudio(string template)
    {
        return _audio.Count == 0 ? template : Replace(template, _audio[0]);
    }

    private static string Replace(string template, IReadOnlyDictionary<string, string> values)
    {
        foreach (var pair in values)
        {
            template = template.Replace(pair.Key, pair.Value, StringComparison.OrdinalIgnoreCase);
        }

        return template;
    }

    private static string SanitizeKey(string value)
    {
        return Regex.Replace(value, "[^A-Za-z0-9]+", string.Empty);
    }
}
