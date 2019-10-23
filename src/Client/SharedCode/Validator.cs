using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace joulukalenteri.Client.SharedCode
{
    public class Validator
    {
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
