using System;
using System.Linq;
using joulukalenteri.Shared;

namespace joulukalenteri.Client.SharedCode
{
    /// <summary>
    /// Shuffles date according to year.
    /// </summary>
    public class DaysShuffler
    {
        private readonly IDateTime datetime;
        /// <summary>
        /// Inject fake datetime for testing purpose.
        /// </summary>
        /// <param name="_datetime"><see cref="IDateTime"/>, which is possibly fake.</param>
        public DaysShuffler(IDateTime _datetime)
        {
            datetime = _datetime;
        }
        /// <summary>
        /// Shuffle Christmas calendar days according to year.
        /// </summary>
        /// <remarks>For same year, it returns always same result. It returns very rarely the same result from different years.</remarks>
        /// <returns>Integer Array of shuffled values</returns>
        public int[] ShuffleDays()
        {
            Random random = new Random(datetime.Now.Year);
            int[] result = Enumerable.Range(1, 25).ToArray();
            for (int i = 0; i < 25; i++)
            {
                Swap(ref result[i], ref result[random.Next(i, 24)]);
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
