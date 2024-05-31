using System;
using System.ComponentModel.Design.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace connection_app
{
    public class GetUsername
    {
        public async Task<string> GetUsernameAsync()
        {
            try
            {
                var handler = new HttpClientHandler(); // Create client handler to ignore SSL vertificate, should be changed
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
                Console.WriteLine($"An error occurred: {ex.Message}"); // Should only be used for debugging
                return null;
            }
        }
    }

    class Program
    {
        public static bool InMatch;

        public static async Task Main(string[] args)
        {
            InMatch = false;
            while (true)
            {
                var app = new GetUsername();
                var username = await app.GetUsernameAsync();

                if (username != null)
                {
                    if (!InMatch)
                    {
                        InMatch = true;
                        Console.WriteLine(InMatch);
                    }
                    Console.WriteLine($"Username: {username}");
                }
                else
                {
                    InMatch = false;
                    Console.WriteLine(InMatch);
                    Console.WriteLine("Failed to retrieve username.");
                }
                await Task.Delay(2000);
            }
        }
    }
}