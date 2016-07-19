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
            DenominiationCounts = new Dictionary<Denomination, int>();
            foreach (Denomination denom in Enum.GetValues(typeof(Denomination)))
            {
                DenominiationCounts.Add(denom, (money != null && money.ContainsKey(denom)) ? money[denom] : 0);
            }           
        }

        public int Amount
        {
            get
            {
                var sum = 0;
                foreach (var denominization in DenominiationCounts)
                {
                    sum += (denominization.Value * (int)denominization.Key);
                }
                return sum;
            }
        }
    }
}
