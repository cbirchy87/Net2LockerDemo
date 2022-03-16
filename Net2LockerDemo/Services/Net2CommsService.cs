using Net2LockerDemo.Services.Models;
using System.Net.Http.Json;

namespace Net2LockerDemo.Services
{
    public class Net2CommsService
    {
        private readonly HttpClient http;
        private const string apibasessl = "https://10.10.74.12:8443/api/v1";
        private const string apibase = "http://127.0.0.1:8080/api/v1";
        

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

        public async Task<string> UpdateUserPin(int userId)
        {
            // await SetAccessToken();
            // http.DefaultRequestHeaders.Add("Authorization", $"{authResponse.token_type} {authResponse.access_token}");
            var updateUser = new Net2User
            {
                id = userId,
                pin = GenerateRandomNo().ToString()
            };
            var newPin = GenerateRandomNo();

            var responce = await http.PutAsJsonAsync($"/api/v1/users/{userId}", updateUser);
            if (responce.IsSuccessStatusCode)
            {
                return updateUser.pin;
            }
            return "";

        }
        private int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        private async Task<bool> CheckPinIsInUser(int pin)
        {
            var users = await GetNet2Users();
            foreach (var item in users)
            {
                if (pin.ToString() == item.pin)
                {
                    return true;
                }
                return false;   
            }
            return false;
        }
    }
}
