using SMSLive247.OpenApi;

namespace SMSLive247.UI.Shared
{
    public class AccountState(AccountResponse account)
    {
        public AccountResponse Account { get; private set; } = account;
        public event Action? OnChange;

        public void Reload()
        {
            //SortColumn = column;
            OnChange?.Invoke();
        }
    }
}
