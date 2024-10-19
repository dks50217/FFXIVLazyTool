namespace FFXIVLazyStore.Model
{
    public class LoginUserInfo
    {
        public required string UserName { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTimeOffset ExpiresAt { get; set; }
    }
}
