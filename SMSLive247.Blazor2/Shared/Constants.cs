using SMSLive247.Blazor2.Components.Utilities;

namespace SMSLive247.UI.Shared
{
    public static class Const
    {
        public static class Routes
        {
            // Auth
            public const string Home = "/";
            public const string Login = "/login";
            public const string Logoff = "/logout";
            public const string Register = "/register";
            public const string ForgotPassword = "/forgot";
            public const string NotFound = "/not-found";

            public static class Legal
            {
                public const string Terms = "/legal/terms.txt";
                public const string Privacy = "/legal/privacy.txt";
            }

            public static class Messaging
            {
                public const string Compose = "/messaging/compose";
                public const string Messages = "/messaging/messages";
                public const string Contacts = "/messaging/contacts";
                public const string SenderIds = "/messaging/sender-ids";
            }

            public static class Account
            {
                public const string Dashboard = "/account/dashboard";
                public const string SubAccounts = "/account/sub-accounts";
                public const string SubAccountDetails = "/account/sub-accounts/{id}";
                public const string Profile = "/account/profile";

                public static string Details(string id) => SubAccountDetails.Replace("{id}", id);
            }

            public static class Wallet
            {
                public const string BuyCredits = "/credits/purchase";
                public const string Deposits = "/credits/deposits";
                public const string Transfers = "/credits/transfers";
                public const string Pricing = "/credits/pricing";
                public const string PurchaseSuccess = "/credits/purchase/success";
            }

            public static class Support
            {
                public const string Coverage = "/support/coverage";
                public const string ContactUs = "/support/contact-us";
                public const string Tickets = "/support/tickets";
            }

            public static class Developer
            {
                public const string ApiDocs = "/developer/api-docs";
                public const string ApiKeys = "/developer/api-keys";
            }
        }

        public static List<NavItem> NavItems =>
        [
            new NavItem
            {
                Items =
                [
                    new NavItem { Title = "Dashboard", Url = Routes.Account.Dashboard, Icon = Icon.IconStyle.Home }
                ]
            },
            new NavItem
            {
                Title = "Messaging",
                Items =
                [
                    new NavItem { Title = "Compose", Url = Routes.Messaging.Compose, Icon = Icon.IconStyle.PenSquare },
                    new NavItem { Title = "Messages", Url = Routes.Messaging.Messages, Icon = Icon.IconStyle.Mail },
                    new NavItem { Title = "Contacts", Url = Routes.Messaging.Contacts, Icon = Icon.IconStyle.Users },
                    new NavItem { Title = "Sender IDs", Url = Routes.Messaging.SenderIds, Icon = Icon.IconStyle.Fingerprint }
                ]
            },
            new NavItem
            {
                Title = "Accounts",
                Items =
                [
                    new NavItem { Title = "Sub-Accounts", Url = Routes.Account.SubAccounts, Icon = Icon.IconStyle.Network },
                    new NavItem { Title = "My Profile", Url = Routes.Account.Profile, Icon = Icon.IconStyle.User }
                ]
            },
            new NavItem
            {
                Title = "Wallet",
                Items =
                [
                    new NavItem { Title = "Buy", Url = Routes.Wallet.BuyCredits, Icon = Icon.IconStyle.CreditCard },
                    //new NavItem { Title = "Deposit", Url = Routes.Wallet.Deposits, Icon = Icon.IconStyle.ArrowRightLeft },
                    new NavItem { Title = "Transfers", Url = Routes.Wallet.Transfers, Icon = Icon.IconStyle.ArrowRightLeft },
                    new NavItem { Title = "Pricing", Url = Routes.Wallet.Pricing, Icon = Icon.IconStyle.Tag }
                ]
            },
            new NavItem
            {
                Title = "Developers",
                Items =
                [
                    new NavItem { Title = "API Keys", Url = Routes.Developer.ApiKeys, Icon = Icon.IconStyle.Terminal },
                    new NavItem { Title = "API Docs", Url = Routes.Developer.ApiDocs, Icon = Icon.IconStyle.BookOpen }
                ]
            },
            new NavItem
            {
                Title = "Support",
                Items =
                [
                    //new Item { Title = "Tickets", Url = "/support/tickets", Icon = Icon.IconStyle.HelpCircle },
                    new NavItem { Title = "Contact Us", Url = Routes.Support.ContactUs, Icon = Icon.IconStyle.HelpCircle },
                    new NavItem { Title = "Coverage", Url = Routes.Support.Coverage, Icon = Icon.IconStyle.Globe }
                ]
            }
        ];

        public class NavItem
        {
            public string? Title { get; init; }
            public string? Url { get; init; }
            public Icon.IconStyle? Icon { get; init; }
            public List<NavItem> Items { get; init; } = [];
        }

    }
}
