namespace SMSLive247.UI.Pages.Messaging.Compose
{
    public static class Extension
    {
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
    }
}
