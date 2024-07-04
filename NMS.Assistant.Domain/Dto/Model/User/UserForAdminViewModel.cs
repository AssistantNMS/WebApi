using System;

namespace NMS.Assistant.Domain.Dto.Model.User
{
    public class UserForAdminViewModel
    {
        public Guid Guid { get; set; }
        public string Username { get; set; }
        public DateTime JoinDate { get; set; }
    }
}