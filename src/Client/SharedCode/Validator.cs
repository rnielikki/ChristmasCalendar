using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace joulukalenteri.Client.SharedCode
{
    public class Validator
    {
        public bool IsOpenToday(int targetDay)
        {
            if (/*DateTime.Today.Month != 12 ||*/ DateTime.Today.Day < targetDay || targetDay<=0 )
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
