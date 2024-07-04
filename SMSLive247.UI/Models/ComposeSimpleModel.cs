using SMSLive247.OpenApi;

namespace SMSLive247.UI.Pages.ViewModels
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
            Contacts = contacts.Select(c => new ContactModel(c)).ToList();
            BatchFiles = batchFiles.Select(b => new ContactModel(b)).ToList();
        }
    }
}
