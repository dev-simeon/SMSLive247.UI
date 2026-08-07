namespace SMSLive247.UI.Shared
{
    public class Settings
    {
        public string BaseUrl { get; set; } = "";
        public string ParentID { get; set; } = "a044b5a7-1147-45cb-871a-76546896c4f3";
        public int SmsMaxParts { get; set; } = 6; // Max SMS part
        public int MaxCharacter { get => SmsMaxParts * 153; } // Max SMS character
    }
}
