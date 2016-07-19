using System;
using System.Collections.Generic;
using System.Linq;

namespace Covarity
{
    public class VendingMachine
    {
        public MoneyBag Money { get; }
        public MoneyBag UserInputtedMoney { get; private set; }

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

        public MoneyBag ReturnChange(int amount)
        {
            if (Money.Amount < amount)
            {
                throw new Exception("Insufficent Funds");
            }

            if (Money.Amount == amount) {
                return Money;
            }

            MoneyBag moneyBag;
            if (CanReturnChange(amount, Money, out moneyBag))
            {
                foreach (var kvp in moneyBag.DenominiationCounts)
                {
                    Money.DenominiationCounts[kvp.Key] -= kvp.Value;
                }

                return moneyBag;
            }
            throw new Exception("Could not return the correct change");
        }
        
        //Determines whether change can be returned, and if so, outputs the corresponding money set.        
        private bool CanReturnChange(int amount, MoneyBag moneyLeft, out MoneyBag moneyBag)
        {
            //Base case, if the amount is 0, we've found a combination that works.
            //Send an empty MoneyBag and let the recursive case handle the amounts
            if (amount == 0)
            {
                moneyBag = new MoneyBag(new Dictionary<Denomination, int>(){
                    {Denomination.One, 0 },
                    {Denomination.Two, 0 },
                    {Denomination.Five, 0 },
                    {Denomination.Ten, 0 },
                });
                return true;
            }

            //Recursive case
            //For each denomination that is still available, and is less or equal than the current amount, try that amount and see if it works
            var denominationsLeft = moneyLeft.DenominiationCounts.Where(z => z.Value > 0 && (int)z.Key <= amount).Select(z => z.Key).ToArray();
            foreach (var denom in denominationsLeft)
            {
                var mb = new MoneyBag(new Dictionary<Denomination, int>(moneyLeft.DenominiationCounts));
                mb.DenominiationCounts[denom]--;
                if (CanReturnChange(amount - (int)denom, mb, out moneyBag))
                {
                    moneyBag.DenominiationCounts[denom]++;
                    return true;
                }
            }

            //The current combination of denominizations doesn't work, we will have to try another one
            moneyBag = null;
            return false;
        }


        private void ResetUserInputtedMoney()
        {
            //Reset all user amounts to 0
            UserInputtedMoney = new MoneyBag(null);
        }
    }
}
