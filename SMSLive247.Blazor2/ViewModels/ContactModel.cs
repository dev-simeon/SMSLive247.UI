using SMSLive247.OpenApi;

namespace SMSLive247.Blazor2.Pages.ViewModels
{
    public record class ContactModel
    {
        public string Key { get; init; }
        public string Name { get; init; }
        public int Count { get; init; }
        public bool Selected { get; set; } = false;
        public bool Visible { get; set; } = true;
        public ContactResponse Response { get; init; }

        public ContactModel() { }

        public ContactModel(string num)
            : this(num, num, 1, null, true) { }

        //public ContactModel(GroupResponse group)
        //    : this(group.GroupName, group.GroupName, group.Members.Count) { }

        public ContactModel(ContactResponse contact) 
            : this(contact.PhoneNumber, contact.ContactName, 1, contact) { }
        public ContactModel(BatchFileResponse batchFile) 
            : this(batchFile.BatchFileID, batchFile.Description, batchFile.TotalNumbers, null) { }

        private ContactModel(string key, string name, int count, ContactResponse? contact, bool selected = false)
        {
            Key = key;
            Name = name;
            Count = count;
            Selected = selected;
            Response = contact ?? new() { ContactName = name, PhoneNumber = key };
        }
    }
}
