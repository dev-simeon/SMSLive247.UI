namespace SMSLive247.UI.Services
{
    public class AlertService
    {
        public event Func<string, string, Task>? OnInfo;
        public event Func<string, string, Task>? OnError;
        public event Func<string, string, Task>? OnSuccess;
        public event Func<string, string, Task<bool>>? OnConfirm;

        public async Task Info(string message, string title = "Information")
        {
            if (OnInfo is not null)
                await OnInfo.Invoke(message, title);
        }

        public async Task Error(string message, string title = "Error")
        {
            if (OnError is not null)
                await OnError.Invoke(message, title);
        }

        public async Task Success(string message, string title = "Success")
        {
            if (OnSuccess is not null)
                await OnSuccess.Invoke(message, title);
        }

        public async Task<bool> Confirm(string message, string title = "Confirm")
        {
            if (OnConfirm is not null)
                return await OnConfirm.Invoke(message, title);

            return false;
        }
    }
}
