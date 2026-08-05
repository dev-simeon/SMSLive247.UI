using SMSLive247.Blazor2.Components.Utilities;

namespace SMSLive247.Blazor2.Components.Layout;

public partial class Sidebar
{
    public class Item
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public Icon.IconStyle Icon { get; set; }
    }

    public class Category
    {
        public string? Title { get; set; }
        public List<Item> Items { get; set; } = [];
    }

    public static List<Category> Categories =>
        [
            new Category
            {
                Items =
                [
                    new Item { Title = "Dashboard", Url = "/account/dashboard", Icon = Icon.IconStyle.Home }
                ]
            },
            new Category
            {
                Title = "Messaging",
                Items =
                [
                    new Item { Title = "Compose", Url = "/messaging/compose", Icon = Icon.IconStyle.PenSquare },
                    new Item { Title = "Messages", Url = "/messaging/messages", Icon = Icon.IconStyle.Mail },
                    new Item { Title = "Contacts", Url = "/messaging/contacts", Icon = Icon.IconStyle.Users },
                    new Item { Title = "Sender IDs", Url = "/messaging/sender-ids", Icon = Icon.IconStyle.Fingerprint }
                ]
            },
            new Category
            {
                Title = "Accounts",
                Items =
                [
                    new Item { Title = "Sub-Accounts", Url = "/account/sub-accounts", Icon = Icon.IconStyle.Network },
                    new Item { Title = "My Profile", Url = "/account/profile", Icon = Icon.IconStyle.User }
                ]
            },
            new Category
            {
                Title = "Wallet",
                Items =
                [
                    new Item { Title = "Buy", Url = "/wallet/buy-credits", Icon = Icon.IconStyle.CreditCard },
                    new Item { Title = "Transfers", Url = "/wallet/transfers", Icon = Icon.IconStyle.ArrowRightLeft },
                    new Item { Title = "Pricing", Url = "/wallet/pricing", Icon = Icon.IconStyle.Tag }
                ]
            },
            new Category
            {
                Title = "Developer",
                Items =
                [
                    new Item { Title = "API Keys", Url = "/developer/api-keys", Icon = Icon.IconStyle.Terminal },
                    new Item { Title = "API Docs", Url = "/developer/api-docs", Icon = Icon.IconStyle.BookOpen }
                ]
            },
            new Category
            {
                Title = "Support",
                Items =
                [
                    new Item { Title = "Contact Us", Url = "/support/contact-us", Icon = Icon.IconStyle.HelpCircle },
                    new Item { Title = "Coverage", Url = "/support/coverage", Icon = Icon.IconStyle.Globe }
                ]
            }
        ];
}
