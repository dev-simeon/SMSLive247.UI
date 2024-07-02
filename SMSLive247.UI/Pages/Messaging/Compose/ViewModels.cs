using SMSLive247.OpenApi;

namespace SMSLive247.UI.Pages.Messaging.Compose
{
    public class ComposeSimpleModel
    {
        public List<string>? SenderIds { get; private set; }  
        public SmsBatchRequest Request { get; private set; } = new();

        public List<ContactModel> Contacts { get; private set; } = [];
        public List<ContactModel> BatchFiles { get; private set; } = [];
        public List<ContactModel> Numbers { get; set; } = [];

        public void Initialize(
            IEnumerable<SenderIdResponse> senderIds,
            IEnumerable<ContactResponse> contacts,
            IEnumerable<BatchFileResponse> batchFiles)
        {
            SenderIds = senderIds.Select(x => x.SenderID).ToList();
            Contacts = contacts.Select(c => new ContactModel(c.PhoneNumber, c.ContactName, 1)).ToList();
            BatchFiles = batchFiles.Select(b => new ContactModel(b.BatchFileID, $"{b.Description} ({b.TotalNumbers})", b.TotalNumbers)).ToList();
        }
    }

    public record class ContactModel
    {
        public string Key { get; init; }
        public string Name { get; init; }
        public int Count { get; init; }
        public bool Selected { get; set; } = false;
        public bool Visible { get; set; } = true;

        public ContactModel(string key, string name, int count, bool selected = false)
        {
            Key = key;
            Name = name;
            Count = count;
            Selected = selected;
        }
    }
}
