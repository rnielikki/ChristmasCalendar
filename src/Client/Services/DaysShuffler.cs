using AdventCalendar.Models;
using AdventCalendar.Settings;
using System;
using System.Linq;

namespace AdventCalendar.Services
{
    /// <summary>
    /// Shuffles date according to year.
    /// </summary>
    public class DaysShuffler
    {
        private readonly IDateTime datetime;
        private readonly int days;
        /// <summary>
        /// Inject fake datetime for testing purpose.
        /// </summary>
        /// <param name="settings">Settings of the application.</param>
        /// <param name="_datetime"><see cref="IDateTime"/>, which is possibly fake.</param>
        public DaysShuffler(IAppSettings settings, IDateTime _datetime)
        {
            datetime = _datetime;
            days = settings.Days;
        }
        /// <summary>
        /// Shuffle Christmas calendar days according to year.
        /// </summary>
        /// <remarks>For same year, it returns always same result. It returns very rarely the same result from different years.</remarks>
        /// <returns>Integer Array of shuffled values</returns>
        public int[] ShuffleDays()
        {
            Random random = new Random(datetime.Now.Year);
            int[] result = Enumerable.Range(1, days).ToArray();
            for (int i = 0; i < days; i++)
            {
                Swap(ref result[i], ref result[random.Next(i, days-1)]);
            }
            return result;

            static void Swap(ref int x, ref int y)
            {
                int temp = x;
                x = y;
                y = temp;
            }
        }
    }
}
