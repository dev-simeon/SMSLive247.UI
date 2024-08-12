using SMSLive247.UI.Components.Data;
using SMSLive247.UI.Services;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace SMSLive247.UI.Pages.Messaging.Compose
{
    public static class Extensions
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
                if (isValid)
                    return column.Ordinal;
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
