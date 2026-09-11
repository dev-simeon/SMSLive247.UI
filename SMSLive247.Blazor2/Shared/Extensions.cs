using Microsoft.JSInterop;
using SMSLive247.OpenApi;

namespace SMSLive247.Blazor
{
    public static class Extensions
    {
        public static async ValueTask DownloadFromStream(this IJSRuntime js, BatchFileResponse file, ApiClient apiClient)
        {
            var response = await apiClient.BatchFileDownloadAsync(file.BatchFileID);
            var safeName = string.IsNullOrEmpty(file.Description) ? file.BatchFileID : file.Description;
            var fileName = $"{safeName}-downloaded-{DateTime.Now:dd-MMM-yyyy}.{file.FileType.ToLower()}";

            await js.DownloadFromStream(fileName, response.Stream);
        }

        public static async ValueTask DownloadFromStream(this IJSRuntime js, string fileName, Stream stream)
        {
            using var streamRef = new DotNetStreamReference(stream, false);
            await js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
    }
}
