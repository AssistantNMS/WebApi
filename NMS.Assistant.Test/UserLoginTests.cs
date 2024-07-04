using NMS.Assistant.Domain.Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMS.Assistant.Test
{
    public class UserLoginTests
    {
        [Test]
        public void GetHash()
        {
            string passwordHash = HashSaltHelper.GetHashString("kurtiscool", "Lenni");

            var t = passwordHash;
        }
    }
}
