using NMS.Assistant.Integration.Mapper;
using NUnit.Framework;

namespace NMS.Assistant.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("01 min 09 sec", 69)]
        [TestCase("01 min 47 sec", 107)]
        [TestCase("02 min 33 sec", 153)]
        [TestCase("03 min 00 sec", 180)]
        [TestCase("03 min 49 sec", 229)]
        [TestCase("04 min 00 sec", 240)]
        [TestCase("04 min 05 sec", 245)]
        [TestCase("03 m 37 s", 217)]
        [TestCase("03m 37s", 217)]
        [TestCase("03m37s", 217)]
        [TestCase("03 37", 217)]
        [TestCase("0337", 217)]
        [TestCase("", 0)]
        public void ConvertRuntimeToInt(string testString, int expected)
        {
            int actualSeconds = RuntimeMapper.ToSeconds(testString);
            Assert.AreEqual(expected, actualSeconds);
        }
    }
}