namespace SMSLive247.UI.Pages.Messaging.Compose
{
    public record class ContactModel(string Key, string Name, int Count)
    {
        public bool Selected { get; set; } = false;
        public bool Visible { get; set; } = true;
    }
}
