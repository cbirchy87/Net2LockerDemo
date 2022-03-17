﻿using Net2LockerDemo.Services.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Net2LockerDemo.Services
{
    public class Net2CommsService
    {
        private readonly HttpClient http;
        private const string apibasessl = "https://10.10.74.12:8443/api/v1";
        private const string apibase = "http://10.10.74.12:8080/api/v1";
        

        private AuthModel auth;

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

        public async Task<AuthResponse> GetAccessToken()
        {
            http.BaseAddress = new Uri(apibase);
            var responce = await http.PostAsJsonAsync($"/api/v1/authorization/tokens", auth);
            if (responce.IsSuccessStatusCode)
            {
                return await responce.Content.ReadFromJsonAsync<AuthResponse>();
            }
            return null;
        }

        public async Task<List<Net2User>> GetNet2Users(AuthResponse authResponse)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/users");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.access_token);
            var response2 = await http.SendAsync(request);

            if (response2.IsSuccessStatusCode)
            {
                var result = await response2.Content.ReadFromJsonAsync<List<Net2User>>();
                return result;
            }
            return null;



            //http.DefaultRequestHeaders.Add("Authorization", $"{authResponse.token_type} {authResponse.access_token}");
            //var responce = await http.GetAsync($"/api/v1/users");
            //if (responce.IsSuccessStatusCode)
            //{
            //    var result = await responce.Content.ReadFromJsonAsync<List<Net2User>>();
            //    return result;
            //}
            //return null;
        }

        public async Task<string> UpdateUserPin(int userId, AuthResponse authResponse)
        {
            var updateUser = new Net2User
            {
                id = userId,
                pin = GenerateRandomNo().ToString()
            };
            var json = JsonConvert.SerializeObject(updateUser);

            var newPin = GenerateRandomNo();

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/users/{userId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.access_token);
            request.Content  = new StringContent(json, Encoding.UTF8, "application/json");

            var respose2 = await http.SendAsync(request);
            if (respose2.IsSuccessStatusCode)
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

        //private async Task<bool> CheckPinIsInUser(int pin)
        //{
        //    var users = await GetNet2Users();
        //    foreach (var item in users)
        //    {
        //        if (pin.ToString() == item.pin)
        //        {
        //            return true;
        //        }
        //        return false;   
        //    }
        //    return false;
        //}
    }
}
