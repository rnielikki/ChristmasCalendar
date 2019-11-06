using System;

namespace joulukalenteri.Client.SharedCode
{
    /// <summary>
    /// Client-side check if the given date is future or not.
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// Check if the day is future or not based on a year and a day.
        /// </summary>
        /// <remarks>Based on <c>IsOpenToday()</c>, the future day is inactivated in the calendar.</remarks>
        /// <param name="targetYear">The year to check</param>
        /// <param name="targetDay">The day to check</param>
        /// <returns><c>true</c> if the date is valid and not the future, otherwise <c>false</c></returns>
        public bool IsOpenToday(int targetYear, int targetDay)
        {
            int currentYear = DateTime.Today.Year;
            if (targetYear < currentYear)
            {
                return true;
            }
            else if (targetYear > currentYear)
            {
                return false;
            }
            else
            {
                return IsOpenToday(targetDay);
            }
        }
        /// <summary>
        /// Check if the day is future or not based on a day in current year.
        /// </summary>
        /// <remarks>Based on <c>IsOpenToday()</c>, the future day is inactivated in the calendar.</remarks>
        /// <param name="targetDay">The day of current year to check</param>
        /// <returns><c>true</c> if the date is valid and not the future, otherwise <c>false</c></returns>
        public bool IsOpenToday(int targetDay)
        {
            if (DateTime.Today.Month != 12 || DateTime.Today.Day < targetDay || targetDay<=0 )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
