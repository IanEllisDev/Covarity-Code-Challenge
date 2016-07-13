using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VendingMachineTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Pass()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Fail()
        {
            Assert.AreEqual(2, 1);
        }
    }
}
