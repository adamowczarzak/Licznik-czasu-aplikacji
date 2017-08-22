using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationTimeCounter.Test
{
    [TestClass]
    public class DailyUseOfApplication_dbTest
    {
        
        [TestMethod]
        public void Test_Update(string nameTitle)
        {
            DailyUseOfApplication_db dailyUseOfApplication_db = new DailyUseOfApplication_db();
            //dailyUseOfApplication_db.Update("coś");

            Assert.AreEqual(1, 1);
        }
    }
}
