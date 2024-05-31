using Blazored.LocalStorage;

namespace SMSLive247.UI.Services
{
    public class StorageService(ILocalStorageService storage)
    {
        //private const string storageKey = "UserSession";

        //public ValueTask SetUserSession(LoginResponse response)
        //{
        //    return storage.SetItemAsync(storageKey, response);
        //}

        //public ValueTask<LoginResponse?> GetUserSession()
        //{
        //    return storage.GetItemAsync<LoginResponse>(storageKey);
        //}

        //public ValueTask ClearUserSession()
        //{
        //    return storage.RemoveItemAsync(storageKey);
        //}

    }
}
