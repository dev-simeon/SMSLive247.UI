namespace SMSLive247.UI
{
    public class Settings
    {
        public string BaseUrl { get; set; } = "";

        public int SmsMaxParts { get; set; } = 6; // Max SMS part
        public int MaxCharacter { get => SmsMaxParts * 153; } // Max SMS character

        public PaystackSettings Paystack { get; set; }
    }

    public class PaystackSettings
    {
        public string Key { get; set; } = "";
    }
}
