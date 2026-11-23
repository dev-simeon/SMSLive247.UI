using SMSLive247.OpenApi;

namespace SMSLive247.UI.Shared
{
    public class AccountState(AccountResponse account)
    {
        public AccountResponse Account { get; private set; } = account;
        public event Action? OnChange;

        public void StateHasChanged()
        {
            OnChange?.Invoke();
        }

        public void UpdateAccount(AccountResponse? account)
        {
            if (account is null)
                return;

            Account = account;
            StateHasChanged();
        }

        public void UpdateBalance(double? balance)
        {
            if (balance is null)
                return;

            Account.WalletBalance = balance.Value;
            StateHasChanged();
        }
    }
}
