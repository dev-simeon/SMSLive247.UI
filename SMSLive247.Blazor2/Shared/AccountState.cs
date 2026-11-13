using SMSLive247.OpenApi;

namespace SMSLive247.UI.Shared
{
    public class AccountState(AccountResponse account)
    {
        public AccountResponse Account { get; private set; } = account;
        public event Action? OnChange;

        public void Reload()
        {
            OnChange?.Invoke();
        }

        public void UpdateBalance(double balance)
        {
            Account.WalletBalance = balance;
            Reload();
        }

        public void UpdateCredits(double credits) => UpdateBalance(credits);
    }
}
