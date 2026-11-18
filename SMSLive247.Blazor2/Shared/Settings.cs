namespace SMSLive247.UI.Shared
{
    public class Settings
    {
        public string BaseUrl { get; set; } = "";
        public string TawkToID { get; set; } = "";
        public string ParentID { get; set; } = "";
        public int SmsMaxParts { get; set; } = 6; // Max SMS part
        public int MaxCharacter { get => SmsMaxParts * 153; } // Max SMS character
        public string GAuthClientId { get; set; } = "";
    }
}
