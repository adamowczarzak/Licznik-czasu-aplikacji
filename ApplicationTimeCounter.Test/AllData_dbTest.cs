using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ApplicationTimeCounter.ApplicationObjectsType;
using ApplicationTimeCounter.Other;

namespace ApplicationTimeCounter.Test
{
    [TestClass]
    public class AllData_dbTest
    {
        [TestMethod]
        public void TestGetDailyActivity()
        {
            RunApplication runApplication = new RunApplication();
            AllData_db allData_db = new AllData_db();
            CommandParameters commandParameters = new CommandParameters();
            commandParameters.StartDate = "2018-01-13";
            commandParameters.EndDate= "2018-01-10";

            List<Activity> dailyActivity = allData_db.GetDailyActivity(commandParameters);
            Assert.IsNotNull(dailyActivity);
        }
    }
}
