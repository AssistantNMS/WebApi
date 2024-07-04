namespace NMS.Assistant.Domain.Contract
{
    public class UserWithToken : User
    {
        public string Token { get; set; }

        public UserWithToken() { }

        public UserWithToken(User user, string token)
        {
            Guid = user.Guid;
            Username = user.Username;
            JoinDate = user.JoinDate;
            Token = token;
        }
    }
}
