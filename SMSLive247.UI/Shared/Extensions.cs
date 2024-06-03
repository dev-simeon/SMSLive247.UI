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
    }
}
