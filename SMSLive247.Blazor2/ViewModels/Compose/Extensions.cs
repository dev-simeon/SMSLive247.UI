using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using SMSLive247.UI.Services;

namespace SMSLive247.Blazor.Pages.ViewModels
{
    public static class ComposeExtensions
    {
        public static List<T> DeepClone<T>(IEnumerable<T> source)
        {
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<List<T>>(json)!;
        }

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

        public static string CountSmsMessages(this string? strSmsText, int smsMaxParts)
        {
            if (string.IsNullOrWhiteSpace(strSmsText))
                return "0 / 160 (1 page)";

            int intSmsLength = strSmsText.GetValidGsmTextLength();
            int intSmsParts = GetMessageParts(intSmsLength);
            int intNextMax = intSmsParts == 1 ? 160 : intSmsParts * 153;

            if (intSmsParts > smsMaxParts)
                return "Maximum SMS characters reached!";

            return $"{intSmsLength} / {intNextMax}  ({intSmsParts} page{(intSmsParts > 1 ? "s" : null)})";
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

        public static int GetValidGsmTextLength(this string smsText)
        {
            smsText = smsText.Replace("\r", "");

            var strGSMTable = "";
            strGSMTable += "@£$¥èéùìòÇØøÅåΔ_ΦΓΛΩΠΨΣΘΞ`ÆæßÉ !\"#¤%&'()*=,-./0123456789:;<=>?¡";
            strGSMTable += "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ`¿abcdefghijklmnopqrstuvwxyzäöñüà\r\n";

            var strExtendedTable = "^{}\\[~]|€";
            var cntGSMOutput = 0;

            for (var i = 0; i < smsText.Length; i++)
            {
                var cPlainText = smsText[i];

                var intGSMTable = strGSMTable.IndexOf(cPlainText);
                if (intGSMTable != -1)
                {
                    cntGSMOutput += 1;
                    continue;
                }
                var intExtendedTable = strExtendedTable.IndexOf(cPlainText);
                if (intExtendedTable != -1)
                    cntGSMOutput += 2;
                else
                    cntGSMOutput += 8;
            }
            return cntGSMOutput;
        }

        public static List<string> ConvertRawUploadToList(this string rawString, int countryCode)
        {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(rawString));
            return stream.ConvertRawUploadToList(countryCode);
        }

        public static List<string> ConvertRawUploadToList(this Stream rawStream, int countryCode)
        {
            int p = 0;
            var sr = new StreamReader(rawStream);
            var currNumber = new List<char>();
            var bulkNumbers = new List<string>();
            var countryCodeArr = countryCode.ToString().ToCharArray();

            while (!(p < 0))
            {
                p = sr.Read();
                //if char code is numeric
                if (p >= 48 & p <= 57)
                {
                    currNumber.Add(Convert.ToChar(p));
                }
                else
                {
                    if (currNumber.Count > 5)
                    {
                        if (currNumber[0] == '0')
                        {
                            currNumber.RemoveRange(0, 1);
                            currNumber.InsertRange(0, countryCodeArr);
                        }
                        bulkNumbers.Add(string.Concat(currNumber.ToArray()));
                    }
                    if (currNumber.Count > 0)
                        currNumber.Clear();
                }
            }
            //take care of the vary last number
            if (currNumber.Count > 5)
            {
                if (currNumber[0] == '0')
                {
                    currNumber.RemoveRange(0, 1);
                    currNumber.InsertRange(0, countryCodeArr);
                }
                bulkNumbers.Add(string.Concat(currNumber.ToArray()));
            }
            //=================================================
            //ohowojeheri ruby

            return bulkNumbers.Distinct().ToList();
        }
    }
}
