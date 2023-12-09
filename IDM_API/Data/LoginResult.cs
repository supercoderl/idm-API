namespace IDM_API.Data
{
    public class LoginResult
    {
        public TokenResult Token { get; set; }
        public RefreshTokenResult RefreshToken { get; set; }
        public UserResult UserResult { get; set; }
    }

    public class TokenResult
    {
        public string AccessToken { get; set; }
        public int ExpirationMinutes { get; set; }
    }

    public class RefreshTokenResult
    {
        public string RefreshToken { get; set; }
        public int ExpirationDays { get; set; }
    }

    public class UserResult
    {
        public Guid UserID { get; set; }
        public string UserNameOrEmail { get; set; }
        public Array Roles { get; set; }
    }
}
