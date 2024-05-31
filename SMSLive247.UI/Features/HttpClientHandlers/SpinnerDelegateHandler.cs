namespace SMSLive247.UI.Services
{
    public class SpinnerDelegateHandler(SpinnerService spinnerService) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //spinnerService.Show();
            //var result = await base.SendAsync(request, cancellationToken);
            //spinnerService.Hide();

            //return result;

            try
            {
                spinnerService.Show();
                return await base.SendAsync(request, cancellationToken);
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
            finally
            {
                spinnerService.Hide();
            }
        }
    }
}
