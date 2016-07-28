using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Covarity;
using System.Collections.Generic;
using System.Linq;

namespace CovarityTests
{
    [TestClass]
    public class MoneyBagTests
    {
        [TestMethod]
        public void MoneyBag_InitializeWithNullParameter_MoneyInitializedAsEmptyDictionary()
        {
            var moneyBag = new MoneyBag(null);
            Assert.IsNotNull(moneyBag.DenominiationCounts);
            Assert.AreEqual(0, moneyBag.DenominiationCounts.Where(z => z.Value > 0).Count());
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
            Assert.AreEqual(expected.Count, moneyBag.DenominiationCounts.Where(z => z.Value > 0).Count());
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

        [TestMethod]
        public void MoneyBag_Amount_NoMoney_AmountIsZero()
        {
            var moneyBag = new MoneyBag(null);
            Assert.AreEqual(0, moneyBag.Amount);
        }

        [TestMethod]
        public void MoneyBag_Amount_SingleOne_AmountIsOne()
        {
            var moneyBag = new MoneyBag(new Dictionary<Denomination, int>() { { Denomination.One, 1 } });
            Assert.AreEqual(1, moneyBag.Amount);
        }

        [TestMethod]
        public void MoneyBag_Amount_SingleTwo_AmountIsTwo()
        {
            var moneyBag = new MoneyBag(new Dictionary<Denomination, int>() { { Denomination.Two, 1 } });
            Assert.AreEqual(2, moneyBag.Amount);
        }

        [TestMethod]
        public void MoneyBag_Amount_SingleFive_AmountIsFive()
        {
            var moneyBag = new MoneyBag(new Dictionary<Denomination, int>() { { Denomination.Five, 1 } });
            Assert.AreEqual(5, moneyBag.Amount);
        }

        [TestMethod]
        public void MoneyBag_Amount_SingleTen_AmountIsTen()
        {
            var moneyBag = new MoneyBag(new Dictionary<Denomination, int>() { { Denomination.Ten, 1 } });
            Assert.AreEqual(10, moneyBag.Amount);
        }

        [TestMethod]
        public void MoneyBag_Amount_MultipleCoins_AmountIsCalculatedCorrectly()
        {
            //Setup
            var ones = 37;
            var twos = 10;
            var fives = 69;
            var tens = 51;
            var expected = (1 * ones) + (2 * twos) + (5 * fives) + (10 * tens);

            //Act
            var moneyBag = new MoneyBag(new Dictionary<Denomination, int>() {
                { Denomination.One, ones },       
                { Denomination.Ten, tens },       
                { Denomination.Five, fives },      
                { Denomination.Two, twos },     
            });

            //Test
            Assert.AreEqual(expected, moneyBag.Amount);
        }
    }
}
