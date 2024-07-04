using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Contract;

namespace NMS.Assistant.Data.Mapper.Domain
{
    public static class UserMapper
    {
        public static User ToDomain(this Persistence.Entity.User dbUser)
        {
            User domain = new User
            {
                Guid = dbUser.Guid,
                Username = dbUser.Username,
                JoinDate = dbUser.JoinDate
            };
            return domain;
        }
    }
}
