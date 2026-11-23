using SMSLive247.OpenApi;

namespace SMSLive247.Blazor.Pages.ViewModels
{
    public abstract class BaseComposeModel<TRequest>
    {
        public bool IsBusy { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string MessageText { get; set; } = string.Empty;
        public DateTimeOffset? DeliveryTime { get; set; } = null;
        public string ClearMessage() => MessageText = string.Empty;
        public string CounterText() => MessageText.CountSmsMessages(5);
        public List<SenderIdResponse> SenderIds { get; protected set; } = [];

        public abstract TRequest ToRequest(DateTimeOffset? deliveryTime);
        public abstract IReadOnlyList<string> GetErrorMessages();
        public abstract long CountRecipients();
        public abstract void ClearSelection();


        //// Message text property with change notification
        //private string _messageText = "";
        //public string MessageText { get => _messageText; set { _messageText = value; NotifyChanged(); } }

        //public event Action? OnMessageChange;
        //public void NotifyChanged() => OnMessageChange?.Invoke();
    }
}
