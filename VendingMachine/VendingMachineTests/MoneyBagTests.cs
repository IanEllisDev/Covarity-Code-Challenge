using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Covarity;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineTests
{
    [TestClass]
    public class MoneyBagTests
    {
        [TestMethod]
        public void MoneyBag_InitializeWithNullParameter_MoneyInitializedAsEmptyDictionary()
        {
            var moneyBag = new MoneyBag(null);
            Assert.IsNotNull(moneyBag.DenominiationCounts);
            Assert.AreEqual(0, moneyBag.DenominiationCounts.Count);
        }

        [TestMethod]
        public void MoneyBag_InitializeWithDictionary_MoneyInitializedWithInput()
        {
            var expected = new Dictionary<Denomination, int>()
            {
                { Denomination.Ten, 500 },
                { Denomination.One, 12 },
                { Denomination.Five, 60 },
            };
            var moneyBag = new MoneyBag(expected);
            Assert.IsNotNull(moneyBag.DenominiationCounts);
            Assert.AreEqual(expected.Count, moneyBag.DenominiationCounts.Count);
            foreach (var expectedItem in expected)
            {
                //Make sure each Item in the inputted parameter exists in the actual MoneyBag object 
                var actualItem = moneyBag.DenominiationCounts.FirstOrDefault(z => z.Key == expectedItem.Key && z.Value == expectedItem.Value);
                Assert.IsNotNull(actualItem, string.Format("Assert failed on denomination check. Expected denominization {0} with count {1}. Actual denominization {2} with count {3}", 
                    expectedItem.Key,
                    expectedItem.Value,
                    actualItem.Key,
                    actualItem.Value));
            }
        }
    }
}
