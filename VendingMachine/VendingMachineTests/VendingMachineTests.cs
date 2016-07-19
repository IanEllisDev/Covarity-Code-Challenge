using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Covarity;

namespace VendingMachineTests
{
    [TestClass]
    public class VendingMachineTests
    {
        [TestMethod]
        public void VendingMachine_Initialize_ValuesAreInitialized()
        {
            var machine = new VendingMachine(null);
            Assert.IsNotNull(machine.Money);
            Assert.IsNotNull(machine.UserInputtedMoney);
            Assert.IsNotNull(machine.Money.DenominiationCounts);
            Assert.IsNotNull(machine.Money.DenominiationCounts);
            Assert.AreEqual(0, machine.Money.DenominiationCounts.Count);
            Assert.AreEqual(0, machine.Money.DenominiationCounts.Count);
        }
    }
}
