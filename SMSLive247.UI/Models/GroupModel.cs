using SMSLive247.OpenApi;

namespace SMSLive247.UI.Pages.ViewModels
{
    public record class GroupModel
    {
        public string Key { get; init; }
        public string Name { get; init; }
        public int Count { get; init; }
        public bool Selected { get; set; } = false;
        public bool Visible { get; set; } = true;
        public GroupResponse Response { get; init; }
        public IEnumerable<ContactModel> Contacts { get; set; } = [];

        public GroupModel(GroupResponse g)
        {
            Key = g.GroupName;
            Name = g.GroupName;
            Count = g.Members.Count;
            Contacts = g.Members.Select(x => new ContactModel(x));
            Response = g;
        }
    }
}
