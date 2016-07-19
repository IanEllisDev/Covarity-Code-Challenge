using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covarity
{
    //Class to keep track of the count of money denominations 
    //as well as some other helpful information (total amount, etc.)
    public class MoneyBag
    {
        public Dictionary<Denomination, int> DenominiationCounts { get; }
        public MoneyBag(Dictionary<Denomination, int> money)
        {
            DenominiationCounts = money ?? new Dictionary<Denomination, int>();
        }
    }
}
