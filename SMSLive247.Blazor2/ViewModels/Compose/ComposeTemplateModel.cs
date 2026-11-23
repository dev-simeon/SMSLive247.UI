using SMSLive247.OpenApi;
using System.Data;
using System.Text;

namespace SMSLive247.Blazor.Pages.ViewModels
{
    public class ComposeTemplateModel : BaseComposeModel<SmsBatchCsvRequest>
    {
        public int? PhoneColumn { get; private set; } = null;
        public BatchFileResponse? SelectedFile { get; private set; } = null;
        public readonly List<BatchFileResponse> BatchFiles = [];
        public readonly List<Dictionary<string, string>> PreviewRows = [];
        public readonly List<string> ColumnNames = [];

        public ComposeTemplateModel(
            IEnumerable<SenderIdResponse> senderIds,
            IEnumerable<BatchFileResponse> batchFiles)
        {
            SenderIds = senderIds.ToList();
            UpdateBatchFiles(batchFiles);
        }

        public override SmsBatchCsvRequest ToRequest(DateTimeOffset? deliveryTime)
        {
            return new SmsBatchCsvRequest()
            {
                BatchFileID = "22",
                PhoneNumberColumn = 0,
                DeliveryTime = deliveryTime,
                MessageText = base.MessageText,
                SenderID = base.SenderId,
            };
        }

        public override IReadOnlyList<string> GetErrorMessages()
        {
            var messages = new List<string>();

            if (SelectedFile is null)
                messages.Add("Please select a data source");

            if (PhoneColumn.HasValue == false)
                messages.Add("Please select a recipient phone column");

            if (CountRecipients() == 0)
                messages.Add("Please select at least one recipient");

            if (string.IsNullOrWhiteSpace(SenderId))
                messages.Add("Please select a sender ID");

            if (string.IsNullOrWhiteSpace(MessageText))
                messages.Add("Please enter a message");

            return messages;
        }

        public override long CountRecipients()
        {
            return SelectedFile?.TotalNumbers ?? 0;
        }

        public override void ClearSelection()
        {
            ClearMessage();
            SenderId = string.Empty;
            ColumnNames.Clear();
            PreviewRows.Clear();
            SelectedFile = null;
            PhoneColumn = null;
        }



        public void SetPhoneColumn(int colIndex)
        {
            PhoneColumn = colIndex;
        }

        public Task SelectBatchFile(Stream stream, BatchFileResponse file, int displayPreview)
        {
            SelectedFile = file;
            return LoadCsvPreview(stream, displayPreview);
        }

        public void UpdateBatchFiles(IEnumerable<BatchFileResponse> batchFiles)
        {
            BatchFiles.Clear();
            BatchFiles.AddRange(batchFiles.Where(x =>
            {
                var type = x.FileType?.ToLowerInvariant();
                return type == "csv" || type == "xls" || type == "xlsx" || type == "xlx";
            }).OrderByDescending(x => x.DateCreated));
        }

        private async Task LoadCsvPreview(Stream stream, int batchSize)
        {
            ColumnNames.Clear();
            PreviewRows.Clear();
            PhoneColumn = null;

            using var reader = new StreamReader(stream, Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true, leaveOpen: true);

            var firstLine = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(firstLine))
                return;

            ColumnNames.AddRange(ReadCsvLine(firstLine));

            string? line;
            while (PreviewRows.Count < batchSize && (line = await reader.ReadLineAsync()) is not null)
            {
                var values = ReadCsvLine(line);

                if (values.Count == 0)
                    continue;

                var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                for (var index = 0; index < ColumnNames.Count; index++)
                    row[ColumnNames[index]] = index < values.Count ? values[index] : "";

                PreviewRows.Add(row);
            }

            PhoneColumn = ColumnNames.FindIndex(x => x.Contains("phone", StringComparison.OrdinalIgnoreCase)
                                                  || x.Contains("mobile", StringComparison.OrdinalIgnoreCase)
                                                  || x.Contains("number", StringComparison.OrdinalIgnoreCase)
                                                  || x.Contains("tel", StringComparison.OrdinalIgnoreCase));

            if (PhoneColumn < 0 && ColumnNames.Count > 0)
                PhoneColumn = 0;
        }

        private static List<string> ReadCsvLine(string line)
        {
            var fields = new List<string>();
            var builder = new StringBuilder();
            var inQuotes = false;

            for (var index = 0; index < line.Length; index++)
            {
                var character = line[index];

                if (character == '"')
                {
                    if (inQuotes && index + 1 < line.Length && line[index + 1] == '"')
                    {
                        builder.Append(character);
                        index++;
                    }
                    else
                        inQuotes = !inQuotes;
                }
                else if (character == ',' && !inQuotes)
                {
                    fields.Add(builder.ToString().Trim());
                    builder.Clear();
                }
                else
                    builder.Append(character);
            }
            fields.Add(builder.ToString().Trim());
            return fields;
        }
    }
}
