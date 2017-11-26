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
        public void GetDailyActivityTest()
        {
            RunApplication runApplication = new RunApplication();
            AllData_db allData_db = new AllData_db();
            CommandParameters commandParameters = new CommandParameters();
            commandParameters.StartDate = "2017-07-15";
            commandParameters.EndDate= "2017-11-26";

            List<Activity> dailyActivity = allData_db.GetDailyActivity(commandParameters);
            Assert.IsNotNull(dailyActivity);
        }
    }
}
