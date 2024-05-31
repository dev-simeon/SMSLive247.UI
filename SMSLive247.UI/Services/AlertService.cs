namespace SMSLive247.UI.Services
{
    public class AlertService
    {
        public event Func<string, string, Task>? OnInfo;
        public event Func<string, string, Task>? OnError;
        public event Func<string, string, Task>? OnSuccess;
        public event Func<string, string, Task<bool>>? OnConfirm;

        public Task Info(string message, string title)
        {
            if (OnInfo != null)
                return OnInfo.Invoke(message, title);
            return Task.CompletedTask;
        }

        public Task Error(string message, string title)
        {
            if (OnError != null)
                return OnError.Invoke(message, title);
            return Task.CompletedTask;
        }

        public Task Success(string message, string title)
        {
            if (OnSuccess != null)
                return OnSuccess.Invoke(message, title);
            return Task.CompletedTask;
        }

        public Task<bool> Confirm(string message, string title)
        {
            if (OnConfirm != null)
                return OnConfirm.Invoke(message, title);
            return Task.FromResult(false);
        }
    }
}
