using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MyTests")]
namespace ApplicationTimeCounter.Test
{
    [TestClass]
    public class DataBaseTest
    {


        [TestMethod]
        public void Test_GetSqlConnection()
        {           
            //DataBase.GetSqlConnection();
            Assert.IsNotNull(DataBase.Connection);
        }

        [TestMethod]
        public void Test_ConnectToDataBase()
        {
            DataBase.ConnectToDataBase();
            string state = DataBase.Connection.State.ToString();

            Assert.AreEqual("Open", state);
        }

        /*
        

        [TestMethod]
        public void Test_UpDateTimeThisTitle()
        {
            int addTime = 30;
            dataBase.NameTitle = "'Brak aktywnego okna'";
            dataBase.command = new SqlCommand();
            dataBase.command.Connection = dataBase.connection;
            dataBase.ConnectToDataBase();
            dataBase.command.CommandText = "SELECT ActivityTime from dailyuseofapplication WHERE Title = " + dataBase.NameTitle;
            SqlDataReader reader = dataBase.command.ExecuteReader();
            int actual = 0;
            int expected = 0;
            while (reader.Read())
            {
                expected = Convert.ToInt32(reader[0]);
            }
            reader.Dispose();

            dataBase.UpDateTimeThisTitle();

            dataBase.command.CommandText = "SELECT ActivityTime from dailyuseofapplication WHERE Title = " + dataBase.NameTitle;
            reader = dataBase.command.ExecuteReader();
            while (reader.Read())
            {
                actual = Convert.ToInt32(reader[0]);
            }
            reader.Dispose();


            Assert.AreEqual((expected+addTime), actual);
        }

        [TestMethod]
        public void Test_AddNameTitleToTableTitles()
        {
            var chars = "abcdefghijklmnopqrstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 20)
                            .Select(s => s[random.Next(s.Length)])
                            .ToArray());

            dataBase.NameTitle = "'" + result.ToString() + "'";
            dataBase.command = new SqlCommand();
            dataBase.command.Connection = dataBase.connection;
            dataBase.ConnectToDataBase();
            dataBase.AddNameTitleToTableTitles();

            string actual = "";
            dataBase.command.CommandText = "SELECT Title from dailyuseofapplication WHERE Title = " + dataBase.NameTitle;
            SqlDataReader reader = dataBase.command.ExecuteReader();
            while (reader.Read())
            {
                actual = reader[0].ToString();
            }
            reader.Dispose();

            Assert.AreEqual(result.ToString(), actual);
        }*/
    }
         
}
