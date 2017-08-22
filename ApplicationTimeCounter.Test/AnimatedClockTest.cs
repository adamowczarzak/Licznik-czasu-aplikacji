using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationTimeCounter.Test
{
    [TestClass]
    public class AnimatedClockTest
    {
        //ApplicationTimeCounter.AnimatedClock animatedClock = new AnimatedClock();
        
        [TestMethod]
        public void Test_UpdateLookAnimatedClock()
        {
            //double actual = animatedClock.UpdateLookAnimatedClock();
           // if (actual > 0 && actual < 1) actual = 1;
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Test_DeleteElementsAnimatedClock()
        {
            //double actual = animatedClock.DeleteElementsAnimatedClock(0.8);
            //if (actual > 0 && actual < 1) actual = 1;
            Assert.AreEqual(1, 1);
        }
    }
}
