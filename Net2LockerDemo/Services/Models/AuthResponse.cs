namespace Net2LockerDemo.Services.Models
{
    public class AuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string expiry_datetime { get; set; }
    }
}
