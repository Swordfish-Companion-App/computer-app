using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace connection_app
{
    public class GetUsername
    {
        public async Task<string?> GetUsernameAsync()
        {
            try
            {
                var handler = new HttpClientHandler(); // Create client handler to ignore SSL certificate, should be changed
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(handler))
                {
                    var response = await client.GetAsync("https://127.0.0.1:2999/liveclientdata/activeplayername");
                    if (response.IsSuccessStatusCode)
                    {
                        var username = await response.Content.ReadAsStringAsync();
                        return username;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex}");
                return null;
            }
        }
    }
}
