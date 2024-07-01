using SMSLive247.OpenApi;
using System.Globalization;

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
    }
}
