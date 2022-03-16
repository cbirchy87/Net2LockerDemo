using Net2LockerDemo.Services.Models;
using System.Net.Http.Json;

namespace Net2LockerDemo.Services
{
    public class Net2CommsService
    {
        private readonly HttpClient http;
        private const string apibasessl = "https://10.10.74.12:8443/api/v1";
        private const string apibase = "http://10.10.74.12:8080/api/v1";
        

        private AuthModel auth;
        private AuthResponse authResponse;

        public Net2CommsService()
        {
            this.http = new HttpClient();
            auth = new AuthModel
            {
                username = "System engineer",
                password = "admin",
                client_id = "bd7073d188fb4d36818ea28a539a6488",
                scope = "offline_access",
                grant_type = "password"
            };
        }

        public async Task SetAccessToken()
        {
            http.BaseAddress = new Uri(apibase);
            var responce = await http.PostAsJsonAsync($"/api/v1/authorization/tokens", auth);
            if (responce.IsSuccessStatusCode)
            {
                authResponse = await responce.Content.ReadFromJsonAsync<AuthResponse>();
            }

        }

        public async Task<List<Net2User>> GetNet2Users()
        {
            await SetAccessToken();
            http.DefaultRequestHeaders.Add("Authorization", $"{authResponse.token_type} {authResponse.access_token}");
            var responce = await http.GetAsync($"/api/v1/users");
            if (responce.IsSuccessStatusCode)
            {
                var result = await responce.Content.ReadFromJsonAsync<List<Net2User>>();
                return result;
            }
            return null;
        }

        public async Task UpdateUserPin(int userId, string newPin)
        {
           // await SetAccessToken();
            http.DefaultRequestHeaders.Add("Authorization", $"{authResponse.token_type} {authResponse.access_token}");
            var updateUser = new Net2User
            {
                id = userId,
                pin = newPin
            };
            var responce = await http.PutAsJsonAsync($"/api/v1/users/{userId}", updateUser);

        }
    }
}
