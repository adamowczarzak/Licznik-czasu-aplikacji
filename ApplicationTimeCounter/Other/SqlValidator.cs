using System.Collections.Generic;
using System.Linq;

namespace ApplicationTimeCounter.Other
{
    static class SqlValidator
    {
        readonly static public string AND = " AND ";
        readonly static public string FromValueEqual = " =< ";
        readonly static public string ToValueEqual = " >= ";
        readonly static public string FromValue = " < ";
        readonly static public string ToValue = " = ";
        readonly static public string ISNULL= " IS NULL ";

        /// <summary>
        /// Metoda waliduje zmienne typu string w zapytaniu sql.
        /// </summary>
        /// <param name="value">Wartość typu string.</param>
        /// <returns>Zwraca zwalidowaną zmienną string.</returns>
        static public string Validate(string value)
        {
            return "'" + value + "'";
        }

        static public string Validate_IN(List<int> values)
        {
            string q = string.Empty;
            for(int i = 0; i < values.Count; i++)
            {
                q += values[i];
                if (values.Count > 1 && i != values.Count - 1) q += " , ";
            }
            return " IN ( " + q + " )";
        }

        static public string Validate_IN(List<string> values)
        {
            string q = string.Empty;
            for (int i = 0; i < values.Count; i++)
            {
                if (!string.IsNullOrEmpty(values[i]))
                {
                    q += Validate(values[i]);
                    if (values.Count > 1 && i != values.Count - 1) q += " , ";
                }
            }
            return " IN ( " + q + " )";
        }

        static public string Validate_BETWEEN(string columnName, string valueFrom, string valueTo)
        {
            return columnName + " BETWEEN " + Validate(valueFrom) + " AND " + Validate(valueTo);
        }
    }
}
