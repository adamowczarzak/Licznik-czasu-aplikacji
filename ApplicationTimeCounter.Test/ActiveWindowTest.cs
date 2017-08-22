using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApplicationTimeCounter.Test
{
    [TestClass]
    public class ActiveWindowTest
    {
        [TestMethod]
        public void Test_GetNameActiveWindow()
        {           
            // Average
            string expected = "ApplicationTimeCounter - Microsoft Visual Studio";
            // Act
            ApplicationTimeCounter.ActiveWindow activeWindow = new ActiveWindow();
            string actual = activeWindow.GetNameActiveWindow();

            //Asert
            Assert.AreEqual(expected, actual);
        }
    }
}
