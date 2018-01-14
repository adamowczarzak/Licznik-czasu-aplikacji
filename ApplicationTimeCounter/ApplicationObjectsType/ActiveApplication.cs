using System.Collections.Generic;
using ApplicationTimeCounter.ApplicationObjectsType;
using System;
using System.Data.SqlClient;

namespace ApplicationTimeCounter
{
    class ActiveApplication
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int ActivityTime { get; set; }
        public string Date { get; set; }
        public string NameActivity { get; set; }
        public IdNameActivityEnum IdNameActivity { get; set; }
        public int IdMembership { get; set; }
        public bool IfAutoGrouping { get; set; }

        public ActiveApplication()
        {
            ID = 0;
            Title = string.Empty;
            ActivityTime = 0;
            Date = string.Empty;
            NameActivity = string.Empty;
            IdNameActivity = IdNameActivityEnum.NonActive;
            IdMembership = 0;
            IfAutoGrouping = false;
        }

        public enum IdNameActivityEnum : int
        {
            NonActive = 0,
            Lack = 1,
            Programming = 2,
            Other = 3
        }

        public static ActiveApplication GetActiveApplicationFromReader(SqlDataReader reader)
        {
            List<string> namesField = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
                namesField.Add(reader.GetName(i));

            ActiveApplication activeApplication = new ActiveApplication();
            if (namesField.Contains(ColumnNames.ID))
                activeApplication.ID = Int32.Parse((reader[ColumnNames.ID]).ToString());

            if (namesField.Contains(ColumnNames.Title))
                activeApplication.Title = reader[ColumnNames.Title].ToString();

            if (namesField.Contains(ColumnNames.NameActivity))
                activeApplication.NameActivity = reader[ColumnNames.NameActivity].ToString();

            if (namesField.Contains(ColumnNames.IdNameActivity))
                activeApplication.IdNameActivity = (ActiveApplication.IdNameActivityEnum)(Int32.Parse((reader[ColumnNames.IdNameActivity]).ToString()));
            
            if (namesField.Contains(ColumnNames.IdMembership))
            {
                int IdMembership = 0;
                if (Int32.TryParse((reader[ColumnNames.IdMembership]).ToString(), out IdMembership))
                    activeApplication.IdMembership = IdMembership;
            }

            if (namesField.Contains(ColumnNames.IfAutoGrouping))
                activeApplication.IfAutoGrouping = reader[ColumnNames.IfAutoGrouping].ToString().Equals("1") ? true : false;
            return activeApplication;
        }

    }
}
