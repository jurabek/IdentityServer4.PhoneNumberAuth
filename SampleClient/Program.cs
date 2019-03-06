using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SampleClient
{
    public class PhoneNumberVerifyResponse
    {
        [JsonProperty("resend_token")] public string ResendToken { get; set; }

        [JsonProperty("verify_token")] public string VerifyToken { get; set; }
    }

    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:62537");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var contentObject = JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                {"phone", "+198989822"}
            });

            var stringContent = new StringContent(contentObject, Encoding.UTF8, "application/json");
            var phoneNumberResult =
                await client.PostAsync("http://localhost:62537/api/verify_phone_number", stringContent);
            var verifyTokenResponseString = await phoneNumberResult.Content.ReadAsStringAsync();
            var verifyTokenResponse =
                JsonConvert.DeserializeObject<PhoneNumberVerifyResponse>(verifyTokenResponseString);

            // request token
            var tokenRequest = new TokenRequest
            {
                GrantType = "phone_number_token",
                ClientId = "phone_number_authentication",
                ClientSecret = "secret",
                Address = disco.TokenEndpoint,
                Parameters = new Dictionary<string, string>
                {
                    {"phone_number", "+198989822"},
                    {"verification_token", verifyTokenResponse.VerifyToken}
                }
            };


            var tokenResponse = await client.RequestTokenAsync(tokenRequest);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("http://localhost:62732/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}