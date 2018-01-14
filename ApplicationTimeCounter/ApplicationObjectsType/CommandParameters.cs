using System.Collections.Generic;
using System.Linq;
using ApplicationTimeCounter.Other;


namespace ApplicationTimeCounter.ApplicationObjectsType
{
    public class CommandParameters
    {
        public List<int> ID { get; set; }
        public List<string> Name { get; set; }
        public string ActivityTimeFrom { get; set; }
        public string ActivityTimeTo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string IfActive { get; set; }
        public string IfConfiguration { get; set; }
        public string IfActiveConfiguration { get; set; }

        public CommandParameters()
        {
            ID = new List<int>();
            Name = new List<string>();
            ActivityTimeFrom = string.Empty;
            ActivityTimeTo = string.Empty;
            StartDate = string.Empty;
            EndDate = string.Empty;
            IfActive = string.Empty;
            IfConfiguration = string.Empty;
            IfActiveConfiguration = string.Empty;
        }

        public static string CheckParameters(CommandParameters parameters)
        {
            string query = string.Empty;
            if (parameters.ID.Any()) query += SqlValidator.AND + ColumnNames.ID + SqlValidator.Validate_IN(parameters.ID);
            if (parameters.Name.Any()) query += SqlValidator.AND + ColumnNames.Name + SqlValidator.Validate_IN(parameters.Name);

            if (!string.IsNullOrEmpty(parameters.ActivityTimeFrom) && !string.IsNullOrEmpty(parameters.ActivityTimeTo))
                query += SqlValidator.AND + SqlValidator.Validate_BETWEEN(ColumnNames.ActivityTime, parameters.ActivityTimeFrom, parameters.ActivityTimeTo);
            if (!string.IsNullOrEmpty(parameters.ActivityTimeFrom) && string.IsNullOrEmpty(parameters.ActivityTimeTo))
                query += SqlValidator.AND + ColumnNames.ActivityTime + SqlValidator.FromValue + parameters.ActivityTimeFrom;
            if (string.IsNullOrEmpty(parameters.ActivityTimeFrom) && !string.IsNullOrEmpty(parameters.ActivityTimeTo))
                query += SqlValidator.AND + ColumnNames.ActivityTime + SqlValidator.ToValue + parameters.ActivityTimeTo;
            
            if (!string.IsNullOrEmpty(parameters.StartDate) && !string.IsNullOrEmpty(parameters.EndDate))
                query += SqlValidator.AND + SqlValidator.Validate_BETWEEN(ColumnNames.Date, parameters.StartDate, parameters.EndDate);
            if (!string.IsNullOrEmpty(parameters.StartDate) && string.IsNullOrEmpty(parameters.EndDate))
                query += SqlValidator.AND + ColumnNames.Date + SqlValidator.FromValue + parameters.StartDate;
            if (string.IsNullOrEmpty(parameters.StartDate) && !string.IsNullOrEmpty(parameters.EndDate))
                query += SqlValidator.AND + ColumnNames.Date + SqlValidator.ToValue + parameters.EndDate;

            if (!string.IsNullOrEmpty(parameters.IfActive))
                query += SqlValidator.AND + ColumnNames.IfActive + SqlValidator.ToValue + parameters.IfActive;
            if (!string.IsNullOrEmpty(parameters.IfConfiguration))
                query += SqlValidator.AND + ColumnNames.IfConfiguration + SqlValidator.ToValue + parameters.IfConfiguration;
            if (!string.IsNullOrEmpty(parameters.IfActiveConfiguration))
                query += SqlValidator.AND + ColumnNames.IfActiveConfiguration + SqlValidator.ToValue + parameters.IfActiveConfiguration;

            return query;
        }
    }
}
