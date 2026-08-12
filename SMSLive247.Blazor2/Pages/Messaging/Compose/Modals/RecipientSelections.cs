namespace SMSLive247.Blazor2.Pages.Messaging.Compose.Modals;

public sealed record SelectedContactRecipient(string Id, string Name, string Phone);

public sealed record SelectedGroupRecipient(string Id, string Name, int Count, List<string> MemberPhones);

public sealed record SelectedFileRecipient(string FileId, string Name, int Count);

public sealed record SelectedRecipientsResult(
    List<SelectedContactRecipient> Contacts,
    List<SelectedGroupRecipient> Groups);
