using System;

namespace ApplicationTimeCounter.Other
{
    static class ActionOnString
    {
        public static string GetFirstLetterFromString(string toSearch)
        {
            int z = 0;
            string firstLetter = string.Empty;
            toSearch += "A";
            while (!Char.IsLetter(toSearch[z])) z++;
            firstLetter = toSearch[z].ToString().ToUpper();

            return firstLetter;
        }
    }
}
