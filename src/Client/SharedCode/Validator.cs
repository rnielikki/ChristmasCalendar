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
            if (/*DateTime.Now.Month != 12 ||*/ DateTime.Now.Day < targetDay || targetDay<=0 )
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
