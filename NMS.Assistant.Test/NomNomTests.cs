using NMS.Assistant.Domain.Helper;
using NUnit.Framework;

namespace NMS.Assistant.Test
{
    public class NomNomTests
    {
        [TestCase("85096021-e7d7-4203-a731-290852b685e0", 3, "A44410D2D76558E31A2EB58D5D48CF5B")]
        [TestCase("85096021-e7d7-4203-a731-290852b685e0", 30, "753375E3A408285EC620264DCA630BE0")]
        public void HashNomNomKey(string nomNomKey, int dayNum, string expected)
        {
            string generatedKey = HashSaltHelper.GetHashString(nomNomKey, dayNum.ToString());
            Assert.AreEqual(generatedKey, expected);
        }
    }
}
