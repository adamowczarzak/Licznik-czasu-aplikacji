using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ApplicationTimeCounter.ApplicationObjectsType
{
    public class Activity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ActivityTime { get; set; }
        public string Date { get; set; }

        public Activity()
        {
            ID = 0;
            Name = string.Empty;
            ActivityTime = 0;
            Date = string.Empty;
        }

        public static Activity GetActivityFromReader(SqlDataReader reader)
        {
            List<string> namesField = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
                namesField.Add(reader.GetName(i));

            Activity activity = new Activity();
            if (namesField.Contains(ColumnNames.ID))
                activity.ID = Int32.Parse((reader[ColumnNames.ID]).ToString());

            if (namesField.Contains(ColumnNames.Name))
                activity.Name = reader[ColumnNames.Name].ToString();

            if (namesField.Contains(ColumnNames.ActivityTime))
                activity.ActivityTime = Int32.Parse((reader[ColumnNames.ActivityTime]).ToString());

            if (namesField.Contains(ColumnNames.Date))
                activity.Date = reader[ColumnNames.Date].ToString();

            return activity;
        }
    }
}
