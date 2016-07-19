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
    }
}
