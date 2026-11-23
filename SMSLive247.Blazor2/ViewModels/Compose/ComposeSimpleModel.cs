using System.Data;
using SMSLive247.OpenApi;

namespace SMSLive247.Blazor.Pages.ViewModels
{
    public class ComposeSimpleModel : BaseComposeModel<SmsBatchRequest>
    {
        public List<ContactModel> Contacts { get; private set; } = [];
        public List<ContactModel> BatchFiles { get; private set; } = [];
        public List<ContactModel> Numbers { get; private set; } = [];

        public void UpdateContacts(List<ContactModel> items) => Contacts = items;
        public void UpdateBulkfiles(List<ContactModel> items) =>  BatchFiles = items;
        public void UpdateNumbers(List<ContactModel> items) => Numbers = items;

        public ComposeSimpleModel(
                IEnumerable<SenderIdResponse> senderIds,
                IEnumerable<ContactResponse> contacts,
                IEnumerable<BatchFileResponse> batchFiles)
        {
            Contacts = contacts.Select(x => new ContactModel(x)).ToList();
            SenderIds = senderIds.ToList();
            BatchFiles = batchFiles
               .Where(x => !string.Equals(x.FileType, "csv", StringComparison.OrdinalIgnoreCase))
               .Select(x => new ContactModel(x)).ToList();
        }

        public override SmsBatchRequest ToRequest(DateTimeOffset? deliveryTime)
        {
            return new SmsBatchRequest()
            {
                BatchFileIDs = BatchFiles.Where(x => x.Selected).Select(x => x.Key).ToList(),
                PhoneNumbers = Contacts.Where(x => x.Selected).Select(x => x.Key).ToList(),
                RawNumbers = string.Join(",", Numbers),
                DeliveryTime = deliveryTime,
                MessageText = base.MessageText,
                SenderID = base.SenderId,
            };
        }

        public override IReadOnlyList<string> GetErrorMessages()
        {
            var messages = new List<string>();

            if (CountRecipients() is not > 0)
                messages.Add("Please select at least one recipient");

            if (string.IsNullOrWhiteSpace(SenderId))
                messages.Add("Please select a sender ID");

            if (string.IsNullOrWhiteSpace(MessageText))
                messages.Add("Please enter a message");

            return messages;
        }

        public override long CountRecipients()
        {
            int c1 = Contacts.Where(x => x.Selected).Sum(x => x.Count);
            int c2 = Numbers.Where(x => x.Selected).Sum(x => x.Count);
            int c3 = BatchFiles.Where(x => x.Selected).Sum(x => x.Count);
            return c1 + c2 + c3;
        }

        public override void ClearSelection()
        {
            ClearMessage();
            SenderId = string.Empty;
            Contacts.ForEach(x => x.Selected = false);
            BatchFiles.ForEach(x => x.Selected = false);
            Numbers.Clear();
        }

    }
}
