namespace TDMaker.Infrastructure.Support;

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

internal static partial class StringUtilities
{
    public static string SanitizeFileName(string value)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var builder = new StringBuilder(value.Length);

        foreach (var character in value)
        {
            builder.Append(invalid.Contains(character) ? '_' : character);
        }

        return WhitespaceRegex().Replace(builder.ToString(), " ").Trim();
    }

    public static string HumanizeBytes(long value)
    {
        string[] units = ["B", "KiB", "MiB", "GiB", "TiB"];
        double size = value;
        var index = 0;
        while (size >= 1024 && index < units.Length - 1)
        {
            size /= 1024;
            index++;
        }

        return string.Create(CultureInfo.InvariantCulture, $"{size:0.##} {units[index]}");
    }

    public static string HumanizeDuration(double milliseconds)
    {
        var duration = TimeSpan.FromMilliseconds(milliseconds);
        return duration.TotalHours >= 1
            ? duration.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture)
            : duration.ToString(@"mm\:ss", CultureInfo.InvariantCulture);
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}
