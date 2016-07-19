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
            var mockData = new Dictionary<Denomination, int>() {
                { Denomination.One, 5 },
                { Denomination.Ten, 100 },
                { Denomination.Five, 62 },
            };

            var machine = new VendingMachine(mockData);
            Assert.IsNotNull(machine.Money);
            Assert.IsNotNull(machine.UserInputtedMoney);
            Assert.IsNotNull(machine.Money.DenominiationCounts);
            Assert.IsNotNull(machine.Money.DenominiationCounts);
            Assert.AreEqual(0, machine.Money.DenominiationCounts.Count);
            Assert.AreEqual(0, machine.Money.DenominiationCounts.Count);
        }
    }
}
