using SMSLive247.OpenApi;

namespace SMSLive247.UI.Pages.ViewModels
{
    public record class ContactModel
    {
        public string Key { get; init; }
        public string Name { get; init; }
        public int Count { get; init; }
        public bool Selected { get; set; } = false;
        public bool Visible { get; set; } = true;
        public ContactResponse Response { get; init; }  

        public ContactModel(string num)
            : this(num, num, 1, null, true) { }

        //public ContactModel(GroupResponse g)
        //    : this(g.GroupName, g.GroupName, g.Members.Count) { }

        public ContactModel(ContactResponse c) 
            : this(c.PhoneNumber, c.ContactName, 1, c) { }
        public ContactModel(BatchFileResponse b) 
            : this(b.BatchFileID, $"{b.Description} ({b.TotalNumbers})", b.TotalNumbers, null) { }

        private ContactModel(string key, string name, int count, ContactResponse? c, bool selected = false)
        {
            Key = key;
            Name = name;
            Count = count;
            Selected = selected;
            Response = c ?? new() { ContactName = name, PhoneNumber = key };
        }
    }
}
