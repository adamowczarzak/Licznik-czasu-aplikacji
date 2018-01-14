using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter.ApplicationObjectsType
{
    public class Membership
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public bool IfActive { get; set; }
        public bool IfConfiguration { get; set; }
        public string Filter { get; set; }
        public bool IfActiveConfiguration { get; set; }
        public bool IfAsOneApplication { get; set; }

        public Membership()
        {
            ID = 0;
            Title = string.Empty;
            Date = string.Empty;
            IfActive = false;
            IfConfiguration = false;
            Filter = string.Empty;
            IfActiveConfiguration = false;
            IfAsOneApplication = false;
        }

        public static Membership GetMembershipFromReader(SqlDataReader reader)
        {
            List<string> namesField = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
                namesField.Add(reader.GetName(i));

            Membership membership = new Membership();
            if (namesField.Contains(ColumnNames.ID))
                membership.ID = Int32.Parse((reader[ColumnNames.ID]).ToString());

            if (namesField.Contains(ColumnNames.Title))
                membership.Title = reader[ColumnNames.Title].ToString();

            if (namesField.Contains(ColumnNames.Date))
                membership.Date = reader[ColumnNames.Date].ToString();

            if (namesField.Contains(ColumnNames.IfActive))
                membership.IfActive = reader[ColumnNames.IfActive].ToString().Equals("1") ? true : false;

            if (namesField.Contains(ColumnNames.IfConfiguration))
                membership.IfConfiguration = reader[ColumnNames.IfConfiguration].ToString().Equals("1") ? true : false;

            if (namesField.Contains(ColumnNames.Filter))
                membership.Filter = reader[ColumnNames.Filter].ToString();

            if (namesField.Contains(ColumnNames.IfActiveConfiguration))
                membership.IfActiveConfiguration = reader[ColumnNames.IfActiveConfiguration].ToString().Equals("1") ? true : false;

            if (namesField.Contains(ColumnNames.IfAsOneApplication))
                membership.IfAsOneApplication = reader[ColumnNames.IfAsOneApplication].ToString().Equals("1") ? true : false;

            return membership;
        }
    }
}
