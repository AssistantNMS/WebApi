using System;

namespace NMS.Assistant.Domain.Dto.Model.User
{
    public class UserViewModel
    {
        public Guid Guid { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }

        public DateTime JoinDate { get; set; }
    }
}
