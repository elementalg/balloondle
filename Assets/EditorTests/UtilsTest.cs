using Balloondle;
using NUnit.Framework;

namespace EditorTests
{
    public class UtilsTest
    {
        [Test]
        public void NegativeIfOddRunsCorrectly()
        {
            Assert.AreEqual(1, Utils.NegativeIfOdd(-2));
            Assert.AreEqual(-1, Utils.NegativeIfOdd(-1));
            Assert.AreEqual(1, Utils.NegativeIfOdd(0));
            Assert.AreEqual(-1, Utils.NegativeIfOdd(1));
            Assert.AreEqual(1, Utils.NegativeIfOdd(2));
            Assert.AreEqual(-1, Utils.NegativeIfOdd(3));
            Assert.AreEqual(1, Utils.NegativeIfOdd(4));
            Assert.AreEqual(-1, Utils.NegativeIfOdd(5));
        }
    }
}