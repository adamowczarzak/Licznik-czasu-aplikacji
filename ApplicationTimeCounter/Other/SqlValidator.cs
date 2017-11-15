using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter.Other
{
    static class SqlValidator
    {
        /// <summary>
        /// Metoda waliduje zmienne typu string w zapytaniu sql.
        /// </summary>
        /// <param name="value">Wartość typu string.</param>
        /// <returns>Zwraca zwalidowaną zmienną string.</returns>
        static public string Validate(string value)
        {
            return "'" + value + "'";
        }
    }
}
