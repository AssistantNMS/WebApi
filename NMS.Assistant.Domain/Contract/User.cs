using System;

namespace NMS.Assistant.Domain.Contract
{
    public class User
    {
        public Guid Guid { get; set; }
        public string Username { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
