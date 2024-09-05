using Newtonsoft.Json;

namespace DAlertsApi.Models.ApiV1.Users
{
    /// <summary>
    /// Obtains user profile information. Requires user authorization with the oauth-user-show scope.
    /// GET:                        https://www.donationalerts.com/api/v1/user/oauth
    /// 
    /// id:                         The unique and unchangeable user identifier
    /// code:                       The unique user name
    /// name:                       The unique displayed user name
    /// avatar:                     The URL to the personalized graphical illustration
    /// email:                      The email address
    /// socket_connection_token:    Centrifugo connection token
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Socket_connection_token { get; set; } = string.Empty;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class UserWrap
    {
        public User Data { get; set; } = new User();
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
