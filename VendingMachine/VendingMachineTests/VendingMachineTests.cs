using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Covarity;
using System.Collections.Generic;

namespace VendingMachineTests
{
    [TestClass]
    public class VendingMachineTests
    {
        [TestMethod]
        public void VendingMachine_InitializeWithNull_ValuesAreInitializedWithNoDenominations()
        {
            var machine = new VendingMachine(null);
            Assert.IsNotNull(machine.Money);
            Assert.IsNotNull(machine.UserInputtedMoney);
            Assert.IsNotNull(machine.Money.DenominiationCounts);
            Assert.IsNotNull(machine.UserInputtedMoney.DenominiationCounts);
            Assert.AreEqual(0, machine.Money.DenominiationCounts.Count);
            Assert.AreEqual(0, machine.UserInputtedMoney.DenominiationCounts.Count);
        }

        [TestMethod]
        public void VendingMachine_InitializeWithValues_ValuesAreInitialized()
        {
            var ones = 5;
            var tens = 100;
            var fives = 62;
            var mockData = new Dictionary<Denomination, int>() {
                { Denomination.One, ones },
                { Denomination.Ten, tens},
                { Denomination.Five, fives },
            };

            var machine = new VendingMachine(mockData);
            Assert.IsNotNull(machine.Money);
            Assert.IsNotNull(machine.UserInputtedMoney);
            Assert.IsNotNull(machine.Money.DenominiationCounts);
            Assert.IsNotNull(machine.UserInputtedMoney.DenominiationCounts);
            Assert.AreEqual(3, machine.Money.DenominiationCounts.Count);
            Assert.AreEqual(0, machine.UserInputtedMoney.DenominiationCounts.Count);

            var denominizations = machine.Money.DenominiationCounts;
            Assert.IsTrue(denominizations.ContainsKey(Denomination.One));
            Assert.IsFalse(denominizations.ContainsKey(Denomination.Two));
            Assert.IsTrue(denominizations.ContainsKey(Denomination.Five));
            Assert.IsTrue(denominizations.ContainsKey(Denomination.Ten));
            Assert.AreEqual(ones, denominizations[Denomination.One]);
            Assert.AreEqual(fives, denominizations[Denomination.Five]);
            Assert.AreEqual(tens, denominizations[Denomination.Ten]);

        }
        
    }
}
