namespace DAlertsApi.Models.ApiV1
{
    public class User
    {
        public int Id { get; set; }
        public string code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Socket_connection_token { get; set; } = string.Empty;

        public override string ToString()
        {
            return "User:\n" +
                   "Id: " + Id + "\n" +
                   "code: " + code + "\n" +
                   "Name: " + Name + "\n" +
                   "Avatar: " + Avatar + "\n" +
                   "Email: " + Email + "\n" +
                   "Socket_connection_token: " + Socket_connection_token + "\n";
        }
    }

    public class UserWrap
    {
        public User Data { get; set; } = new User();
    }
}
