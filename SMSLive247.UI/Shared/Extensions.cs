using SMSLive247.OpenApi;
using System.Globalization;
using System.Text;

namespace SMSLive247.UI
{
    public static class Extensions
    {
        private static readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        public static string Left(this string input, int length)
        {
            if (input == null)
                return string.Empty;

            if (input.Length > length)
                return $"{input[..length]}...";

            return input;
        }

        public static string Capitalize(this string input) 
        {
            if (input == null) 
                return string.Empty;
            return _textInfo.ToTitleCase(input.ToLower());
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
                {
                    cntGSMOutput += 2;
                }
                else
                {
                    cntGSMOutput += 8;
                }
            }
            return cntGSMOutput;
        }

        public static string FormatApiMessage(this Exception exception)
        {
            switch (exception)
            {
                //case ApiException<ApiErrorResponse> ex:
                //    {
                //        if (ex.StatusCode == 200)
                //            return "Operation was successful";

                //        if (ex.Result.Errors == null)
                //            return $"<p>{ex.Result.Message}</p>";

                //        var message = $"<p><b>{ex.Result.Message}</b></p><hr/>";

                //        foreach (var item in ex.Result.Errors)
                //        {
                //            message += $"<p><b>{item.Field}</b>: {item.Message}</p>";
                //        }
                //        return message;
                //    }
                case ApiException ex:
                    {
                        if (ex.StatusCode >= 200 && ex.StatusCode < 300)
                            return "<p>Operation was successful, but software error.</p>" +
                                "<p>TODO: why this exception?</p>";

                        return $"<p>{ex.Message}</p>";
                    }
                case HttpRequestException ex:
                    {
                        return $"<p>Network Error.</p><hr/>" +
                            $"<p>Please check your Internet connection.</p> " +
                            $"{ex.Message}";
                    }
                default:
                    return exception.Message;
            }
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
                //U.InputStream.ReadByte
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
