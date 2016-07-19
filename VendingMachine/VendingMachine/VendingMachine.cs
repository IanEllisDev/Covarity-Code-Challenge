using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covarity
{
    public class VendingMachine
    {
        public MoneyBag Money { get; }
        public MoneyBag UserInputtedMoney { get; }

        public VendingMachine(Dictionary<Denomination, int> initialMoneyAmounts)
        {
            Money = new MoneyBag(initialMoneyAmounts);
            UserInputtedMoney = new MoneyBag(null);
        }

        public void AcceptMoney(Denomination d)
        {
            if (!Money.DenominiationCounts.ContainsKey(d))
            {
                Money.DenominiationCounts.Add(d, 0);
            }
            
            if (!UserInputtedMoney.DenominiationCounts.ContainsKey(d))
            {
                UserInputtedMoney.DenominiationCounts.Add(d, 0);
            }

            UserInputtedMoney.DenominiationCounts[d]++;
        }

        public MoneyBag ReturnMoney()
        {
            var ret = new MoneyBag(new Dictionary<Denomination, int>(UserInputtedMoney.DenominiationCounts));
            ResetUserInputtedMoney();
            return ret;
        }

        private void ResetUserInputtedMoney()
        {
            //Reset all values to 0
            var keys = UserInputtedMoney.DenominiationCounts.Select(z => z.Key).ToArray();
            foreach (var key in keys)
            {
                UserInputtedMoney.DenominiationCounts[key] = 0;
            }
        }
    }
}
