using System;

namespace ApplicationTimeCounter.Other
{
    static class ActionOnTime
    {
        public static string GetTime(int timeInMinutes)
        {
            TimeSpan result = TimeSpan.FromMinutes(timeInMinutes);
            return result.ToString("h':'m").Replace(":", " h ") + " min";
        }

        public static string GetTime()
        {
            TimeSpan result = TimeSpan.FromMinutes(GetNowTimeInMinuts());
            return result.ToString("h':'m").Replace(":", " h ") + " min";
        }

        public static string GetTimeAndDays(int timeInMinutes)
        {
            TimeSpan result = TimeSpan.FromMinutes(timeInMinutes);
            TimeSpan day = TimeSpan.FromDays(1);
            if (result > day)
                return result.ToString("d'?:'h':'m").Replace("?:", " d ").Replace(":", " h ") + " min";
            else
                return result.ToString("h':'m").Replace(":", " h ") + " min";
        }

        public static double GetNowTimeInMinuts()
        {
            string m = DateTime.Now.ToString("mm");
            string h = DateTime.Now.ToString("HH");
            return (60 * Convert.ToInt32(h) + Convert.ToInt32(m)) + 1;
        }

        public static string GetRemainingTime()
        {
            int time = (24 * 60) - (Convert.ToInt32(GetNowTimeInMinuts()));
            return GetTime(time);
        }

        public static string GetRemainingTimePercent()
        {
            int percent = Convert.ToInt32(((24 * 60) - ActionOnTime.GetNowTimeInMinuts()) / (24 * 60) * 100);
            return percent + " %";
        }
    }
}
