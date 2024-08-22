using CsvHelper;
using SMSLive247.OpenApi;
using SMSLive247.UI.Services;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SMSLive247.UI.Pages.ViewModels
{
    public enum SendOption { SEND_NOW, SEND_LATER }

    public class ComposeSimpleModel
    {
        public string CounterText { get; set; } = "Type your Message here";
        public SendOption SendOption { get; set; } = SendOption.SEND_NOW;

        public List<string>? SenderIds { get; private set; }  
        public SmsBatchRequest Request { get; private set; } = new();

        public List<ContactModel> Contacts { get; private set; } = [];
        public List<ContactModel> BatchFiles { get; private set; } = [];
        public List<ContactModel> Numbers { get; set; } = [];

        public ComposeSimpleModel(
            IEnumerable<SenderIdResponse> senderIds,
            IEnumerable<ContactResponse> contacts,
            IEnumerable<BatchFileResponse> batchFiles)
        {
            SenderIds = senderIds.Select(x => x.SenderID).ToList();
            Contacts = contacts.Select(x => new ContactModel(x)).ToList();
            BatchFiles = batchFiles.Select(x => new ContactModel(x)).ToList();

            Request.DeliveryTime = DateTime.Now;
        }
    }

    public class ComposeTemplateModel
    {
        public string DeliveryEmail = string.Empty; //TODO; add to API model
        public string CounterText { get; set; } = "Type your Message here";
        public SendOption SendOption { get; set; } = SendOption.SEND_NOW;

        public List<string>? SenderIds { get; private set; }
        public SmsBatchCsvRequest Request { get; private set; } = new();


        public List<BatchFileResponse> BatchCsvFiles { get; private set; } = [];
        public List<DataColumn> DataColumns => dataTable.Columns.Cast<DataColumn>().ToList();
        public List<DataRow> DataRows => dataTable.Rows.Cast<DataRow>().ToList();
        public bool IsValidPhoneColumn => dataTable.IsPhoneNumberColumn(Request.PhoneNumberColumn);
        //public bool IsLoaded => dataTable.Columns.Count > 0;

        private readonly DataTable dataTable = new();

        public ComposeTemplateModel(
            IEnumerable<SenderIdResponse> senderIds,
            IEnumerable<BatchFileResponse> batchFiles)
        {
            SenderIds = senderIds.Select(x => x.SenderID).ToList();
            BatchCsvFiles = batchFiles.Where(x => x.FileType == "csv").OrderByDescending(x => x.DateCreated).ToList();
            Request.DeliveryTime = DateTime.Now;
        }

        public void LoadData(Stream stream, string batchId)
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            using var dr = new CsvDataReader(csv);

            dataTable.Load(dr);
            // Detect the column index for phone numbers
            Request.PhoneNumberColumn = dataTable.DetectPhoneNumberColumn();
            Request.BatchFileID = batchId;
        }

        public void ClearData()
        {
            dataTable.Clear();
        }

        public void ClearMessage()
        {
            Request.MessageText = string.Empty;
            CounterText = "Type your Message here";
        }

    }

    public static class ComposeExtensions
    {
        public static bool IsPhoneNumberColumn(this DataTable records, int columnIndex)
        {
            return records.Rows.OfType<DataRow>().ToList().All(row => IsPhoneNumber(row[columnIndex].ToString()));
        }

        public static int DetectPhoneNumberColumn(this DataTable records)
        {
            var columns = records.Columns.Cast<DataColumn>().ToList();

            foreach (DataColumn column in columns)
            {
                var isValid = records.IsPhoneNumberColumn(column.Ordinal);
                if (isValid) return column.Ordinal;
            }
            return -1; // Return -1 if no phone number column is detected
        }

        public static bool IsPhoneNumber(this string? value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            // Trim spaces from the value
            value = value.Trim();

            // Check if the value matches a common phone number pattern
            // Length check: assuming phone numbers are between 7 and 15 digits long
            // This regex allows for optional country code with +, spaces, dashes, and parentheses
            var phonePattern = @"^(\+?\d{1,4}[-.\s]?)?(\(?\d{1,4}\)?[-.\s]?)?(\d{7,15})$";

            return Regex.IsMatch(value, phonePattern);
        }

        public static string RemoveSpacesBetweenBraces(this string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var sourceParts = input.Split(['{', '}']);
            var isEvenPositions = !input.StartsWith("{");

            for (var p = 1; p <= sourceParts.Length; p++)
            {
                if (p % 2 == 0 & isEvenPositions)
                    sourceParts[p - 1] = "{" + sourceParts[p - 1].Replace(" ", "").ToUpper() + "}";
            }
            return string.Join("", sourceParts);
        }

        public static async Task<string> CountSmsMessages(AlertService alert, string? strSmsText, int smsMaxParts)
        {
            if (string.IsNullOrWhiteSpace(strSmsText))
            {
                return "Type your Message here";
            }

            int intSmsLength = strSmsText.GetValidGsmTextLength();
            int intSmsParts = GetMessageParts(intSmsLength);
            int intNextMax = intSmsParts == 1 ? 160 : intSmsParts * 153;

            if (intSmsParts > smsMaxParts)
            {
                await alert.Info("Maximum SMS characters reached!", "alert");
                return string.Empty;
            }

            if (intSmsParts > 1)
            {
                if (intSmsLength == 161)
                    await alert.Info($"You have just exceeded 160 characters. You will be charged {intSmsParts} pages for this message!", "alert");

                if (intSmsLength == intNextMax - 153 + 1)
                    await alert.Info($"You have just exceeded {intNextMax - 153} characters. You will be charged {intSmsParts} pages for this message!", "alert");
            }
            return $"{intSmsLength} / {intNextMax} . . . . . . {intSmsParts} page{(intSmsParts > 1 ? "s" : null)}";
        }

        public static int GetMessageParts(int length)
        {
            if (length <= 160) return 1;

            return (int)Math.Ceiling(length / 153.0);
        }
    }

}
