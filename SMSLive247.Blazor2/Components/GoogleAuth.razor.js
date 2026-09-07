// JavaScript for GoogleAuth component
export function signIn(dotNetObject, clientId, methodName) {
    return new Promise((resolve, reject) => {
        google.accounts.id.initialize({
            client_id: clientId,
            callback: (response) => {
                dotNetObject.invokeMethodAsync(methodName, response.credential);
                resolve(response.credential);
            }
        });
        google.accounts.id.prompt();
    });
}