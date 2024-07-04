using NMS.Assistant.Domain.Helper;
using NUnit.Framework;

namespace NMS.Assistant.Test
{
    public class GunterTests
    {
        [TestCase("77ccd627-f588-42b0-aa37-8836232f947f", 1, "DFF17533A8676DB7280B60F571EE7702")]
        [TestCase("77ccd627-f588-42b0-aa37-8836232f947f", 2, "9E4C13DB9C3E596F0060238B9BF87383")]
        [TestCase("77ccd627-f588-42b0-aa37-8836232f947f", 3, "EB9E97CAA828276CD34FD2ED8394F75B")]
        //[TestCase("77ccd627-f588-42b0-aa37-8836232f947f", 30, "04B628EE85AB3F66F3FC39A1BBEA9DD7")]
        public void HashGunterKey(string nomNomKey, int dayNum, string expected)
        {
            string generatedKey = HashSaltHelper.GetHashString(nomNomKey, dayNum.ToString());
            Assert.AreEqual(generatedKey, expected);
        }
    }
}
