using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TDMaker.Cli;
using TDMaker.Infrastructure.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});
builder.Services.AddTDMakerInfrastructure();
builder.Services.AddSingleton<CliApplication>();

using var host = builder.Build();

var app = host.Services.GetRequiredService<CliApplication>();
return await ExecuteAsync(args, app);

static async Task<int> ExecuteAsync(string[] args, CliApplication app)
{
    if (args.Length == 0)
    {
        WriteUsage();
        return 1;
    }

    var command = args[0].Trim().ToLowerInvariant();
    var parsed = ParsedArguments.Parse(args.Skip(1));

    if (parsed.Errors.Count > 0)
    {
        foreach (var error in parsed.Errors)
        {
            Console.Error.WriteLine(error);
        }

        Console.Error.WriteLine();
        WriteUsage();
        return 1;
    }

    if (parsed.ShowHelp)
    {
        WriteUsage();
        return 0;
    }

    return command switch
    {
        "tools" => await app.ShowToolsAsync(),
        "inspect" => await ExecuteInspectAsync(parsed, app),
        "run" => await ExecuteRunAsync(parsed, app),
        _ => WriteUnknownCommand(command)
    };
}

static async Task<int> ExecuteInspectAsync(ParsedArguments parsed, CliApplication app)
{
    if (parsed.Inputs.Count == 0)
    {
        Console.Error.WriteLine("The inspect command requires at least one input path.");
        return 1;
    }

    return await app.InspectAsync(
        parsed.Inputs,
        parsed.ProfileId,
        parsed.Title,
        parsed.Source);
}

static async Task<int> ExecuteRunAsync(ParsedArguments parsed, CliApplication app)
{
    if (parsed.Inputs.Count == 0)
    {
        Console.Error.WriteLine("The run command requires at least one input path.");
        return 1;
    }

    return await app.RunAsync(
        parsed.Inputs,
        parsed.ProfileId,
        parsed.Title,
        parsed.Source,
        parsed.OutputDirectory,
        parsed.Screenshots,
        parsed.Upload,
        parsed.Torrent,
        parsed.WriteXml);
}

static int WriteUnknownCommand(string command)
{
    Console.Error.WriteLine($"Unknown command '{command}'.");
    Console.Error.WriteLine();
    WriteUsage();
    return 1;
}

static void WriteUsage()
{
    Console.WriteLine("""
TDMaker CLI

Usage:
  tdmaker tools
  tdmaker inspect [options] <input> [<input>...]
  tdmaker run [options] <input> [<input>...]

Options:
  -p, --profile <id>     Profile id from settings.json
      --title <value>    Override release title
      --source <value>   Override source label
  -o, --output <path>    Override output directory for run
      --screenshots <bool>
      --upload <bool>
      --torrent <bool>
      --xml              Write legacy XML output during run
  -h, --help             Show help
""");
}

file sealed class ParsedArguments
{
    public List<string> Inputs { get; } = [];
    public List<string> Errors { get; } = [];
    public string? ProfileId { get; private set; }
    public string? Title { get; private set; }
    public string? Source { get; private set; }
    public string? OutputDirectory { get; private set; }
    public bool? Screenshots { get; private set; }
    public bool? Upload { get; private set; }
    public bool? Torrent { get; private set; }
    public bool WriteXml { get; private set; }
    public bool ShowHelp { get; private set; }

    public static ParsedArguments Parse(IEnumerable<string> args)
    {
        var parsed = new ParsedArguments();
        using var enumerator = args.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            if (string.IsNullOrWhiteSpace(current))
            {
                continue;
            }

            if (!current.StartsWith('-'))
            {
                parsed.Inputs.Add(current);
                continue;
            }

            switch (current)
            {
                case "-h":
                case "--help":
                    parsed.ShowHelp = true;
                    break;
                case "-p":
                case "--profile":
                    parsed.ProfileId = parsed.ReadValue(enumerator, current);
                    break;
                case "--title":
                    parsed.Title = parsed.ReadValue(enumerator, current);
                    break;
                case "--source":
                    parsed.Source = parsed.ReadValue(enumerator, current);
                    break;
                case "-o":
                case "--output":
                    parsed.OutputDirectory = parsed.ReadValue(enumerator, current);
                    break;
                case "--screenshots":
                    parsed.Screenshots = parsed.ReadBool(enumerator, current);
                    break;
                case "--upload":
                    parsed.Upload = parsed.ReadBool(enumerator, current);
                    break;
                case "--torrent":
                    parsed.Torrent = parsed.ReadBool(enumerator, current);
                    break;
                case "--xml":
                    parsed.WriteXml = true;
                    break;
                default:
                    parsed.Errors.Add($"Unknown option '{current}'.");
                    break;
            }
        }

        return parsed;
    }

    private string? ReadValue(IEnumerator<string> enumerator, string optionName)
    {
        if (!enumerator.MoveNext() || string.IsNullOrWhiteSpace(enumerator.Current))
        {
            Errors.Add($"Option '{optionName}' requires a value.");
            return null;
        }

        return enumerator.Current;
    }

    private bool? ReadBool(IEnumerator<string> enumerator, string optionName)
    {
        var raw = ReadValue(enumerator, optionName);
        if (raw is null)
        {
            return null;
        }

        var parsed = ParseNullableBool(raw);
        if (!parsed.HasValue)
        {
            Errors.Add($"Option '{optionName}' expects true/false.");
        }

        return parsed;
    }

    private static bool? ParseNullableBool(string value)
    {
        return value.Trim().ToLowerInvariant() switch
        {
            "true" or "1" or "yes" or "on" => true,
            "false" or "0" or "no" or "off" => false,
            _ => null
        };
    }
}
