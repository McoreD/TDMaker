namespace TDMaker.Infrastructure.Uploads;

using System.Text.Json;
using Microsoft.Extensions.Logging;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public sealed class PtpImgUploadService(
    IHttpClientFactory httpClientFactory,
    ILogger<PtpImgUploadService> logger) : IImageUploadService
{
    private const string UploadEndpoint = "https://ptpimg.me/upload.php";

    public async Task<IReadOnlyList<ScreenshotArtifact>> UploadAsync(
        IReadOnlyList<ScreenshotArtifact> screenshots,
        ReleaseProfile profile,
        CancellationToken cancellationToken = default)
    {
        if (screenshots.Count == 0 || string.IsNullOrWhiteSpace(profile.PtpImgApiKey))
        {
            return screenshots;
        }

        var client = httpClientFactory.CreateClient(nameof(PtpImgUploadService));
        var uploaded = new List<ScreenshotArtifact>(screenshots.Count);

        foreach (var screenshot in screenshots)
        {
            try
            {
                using var form = new MultipartFormDataContent
                {
                    { new StringContent(profile.PtpImgApiKey), "api_key" },
                    { new StringContent("json"), "format" }
                };

                await using var stream = File.OpenRead(screenshot.LocalPath);
                using var fileContent = new StreamContent(stream);
                form.Add(fileContent, "file-upload[0]", Path.GetFileName(screenshot.LocalPath));

                using var response = await client.PostAsync(UploadEndpoint, form, cancellationToken);
                response.EnsureSuccessStatusCode();

                var payload = await response.Content.ReadAsStringAsync(cancellationToken);
                var (remoteUrl, thumbnailUrl) = ParseResponse(payload);

                uploaded.Add(new ScreenshotArtifact
                {
                    LocalPath = screenshot.LocalPath,
                    Timestamp = screenshot.Timestamp,
                    RemoteUrl = remoteUrl,
                    ThumbnailUrl = thumbnailUrl
                });
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Could not upload screenshot {Path} to ptpimg", screenshot.LocalPath);
                uploaded.Add(screenshot);
            }
        }

        return uploaded;
    }

    private static (string? remoteUrl, string? thumbnailUrl) ParseResponse(string payload)
    {
        using var document = JsonDocument.Parse(payload);
        var first = document.RootElement.EnumerateArray().FirstOrDefault();
        var code = first.TryGetProperty("code", out var codeElement) ? codeElement.GetString() : null;
        var extension = first.TryGetProperty("ext", out var extElement) ? extElement.GetString() : null;

        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(extension))
        {
            return (null, null);
        }

        var remoteUrl = $"https://ptpimg.me/{code}.{extension}";
        return (remoteUrl, remoteUrl);
    }
}
