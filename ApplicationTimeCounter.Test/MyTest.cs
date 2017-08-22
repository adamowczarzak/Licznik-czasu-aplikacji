using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationTimeCounter.Test
{
    [TestClass]
    public class MyTest
    {
        [TestMethod]
        public void TestMyTest()
        {
            int actual = Convert.ToInt32(Math.Round(0.31 * 79));
            int expected = 24;

            Assert.AreEqual(expected, actual);
        }
    }
}
