using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Covarity;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineTests
{
    [TestClass]
    public class VendingMachineTests
    {
        VendingMachine machine;
        [TestInitialize]
        public void Init()
        {
            machine = null;
        }
        
        private void DefaultInitialization()
        {
            var ones = 5;
            var tens = 100;
            var fives = 62;
            var mockData = new Dictionary<Denomination, int>() {
                { Denomination.One, ones },
                { Denomination.Ten, tens },
                { Denomination.Five, fives },
            };

            machine = new VendingMachine(mockData);            
        }

        #region Initialization

        [TestMethod]
        public void VendingMachine_InitializeWithNull_ValuesAreInitializedWithNoDenominations()
        {
            machine = new VendingMachine(null);
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
            //Could use DefaultInitialization here, but we should make sure to test the values of Money to make sure they stick.
            var ones = 5;
            var tens = 100;
            var fives = 62;
            var mockData = new Dictionary<Denomination, int>() {
                { Denomination.One, ones },
                { Denomination.Ten, tens },
                { Denomination.Five, fives },
            };

            machine = new VendingMachine(mockData);

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

        #endregion

        #region Accept Money

        [TestMethod]
        public void VendingMachine_AcceptMoney_UnknownDenominization_MoneyIsAcceptedAndNewDenominizationIsInitialized()
        {
            //Setup
            var unknownDenom = Denomination.Two;
            DefaultInitialization();

            //Make sure the denominization doesn't exist 
            if (machine.Money.DenominiationCounts.ContainsKey(unknownDenom) || machine.UserInputtedMoney.DenominiationCounts.ContainsKey(unknownDenom))
            {
                Assert.Inconclusive("Test Initialization Failure. Was expecting Denominization.Two to be absent from initial list");
            }

            //Act
            machine.AcceptMoney(unknownDenom);

            //Test
            Assert.IsTrue(machine.Money.DenominiationCounts.ContainsKey(unknownDenom)); 
            Assert.IsTrue(machine.UserInputtedMoney.DenominiationCounts.ContainsKey(unknownDenom));
            Assert.AreEqual(1, machine.UserInputtedMoney.DenominiationCounts[unknownDenom]);
        }

        [TestMethod]
        public void VendingMachine_AcceptMoney_KnownDenominization_MoneyIsAccepted()
        {
            //Setup
            var knownDenom = Denomination.Five;
            DefaultInitialization();

            //Make sure the denominization doesn't exist 
            if (!(machine.Money.DenominiationCounts.ContainsKey(knownDenom)))
            {
                Assert.Inconclusive("Test Initialization Failure. Was expecting Denominization to be known from initial list");
            }

            //Act
            machine.AcceptMoney(knownDenom);

            //Test
            Assert.IsTrue(machine.Money.DenominiationCounts.ContainsKey(knownDenom));
            Assert.IsTrue(machine.UserInputtedMoney.DenominiationCounts.ContainsKey(knownDenom));
            Assert.AreEqual(1, machine.UserInputtedMoney.DenominiationCounts[knownDenom]);
        }

        [TestMethod]
        public void VendingMachine_AcceptMoney_AddMultipleCoins_CoinsAreAdded()
        {
            //Setup
            DefaultInitialization();
            
            //Act
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Five);
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Ten);
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Two);
            machine.AcceptMoney(Denomination.Five);
            machine.AcceptMoney(Denomination.Ten);
            machine.AcceptMoney(Denomination.One);


            //Test
            Assert.AreEqual(4, machine.UserInputtedMoney.DenominiationCounts[Denomination.One]);
            Assert.AreEqual(1, machine.UserInputtedMoney.DenominiationCounts[Denomination.Two]);
            Assert.AreEqual(2, machine.UserInputtedMoney.DenominiationCounts[Denomination.Five]);
            Assert.AreEqual(2, machine.UserInputtedMoney.DenominiationCounts[Denomination.Ten]);            
        }

        [TestMethod]
        public void VendingMachine_AcceptMoney_MachineMoneyCountsAreUnaffected()
        {
            //Setup
            DefaultInitialization();
            var oldMoney = new MoneyBag(new Dictionary<Denomination, int>(machine.Money.DenominiationCounts));

            //Act
            machine.AcceptMoney(Denomination.One);

            //Test
            Assert.AreEqual(oldMoney.DenominiationCounts[Denomination.One], machine.Money.DenominiationCounts[Denomination.One]);
        }

        #endregion

        #region Return Money

        [TestMethod]
        public void VendingMachine_ReturnMoney_NoMoneyInserted_EmptyMoneyBagReturned()
        {
            //Setup
            DefaultInitialization();
            
            //Act
            var actual = machine.ReturnMoney();

            //Test
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.DenominiationCounts.Count);
        }

        [TestMethod]
        public void VendingMachine_ReturnMoney_SingleDenominizationAdded_MoneyBagContainsInsteredDenominization()
        {
            //Setup
            var denom = Denomination.Ten;
            DefaultInitialization();
            machine.AcceptMoney(denom);

            //Act
            var actual = machine.ReturnMoney();

            //Test
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.DenominiationCounts.Count);
            Assert.AreEqual(1, actual.DenominiationCounts[denom]);
        }


        [TestMethod]
        public void VendingMachine_ReturnMoney_MultipleDenominizationAdded_MoneyBagContainsInsteredDenominization()
        {
            //Setup
            DefaultInitialization();
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Five);
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Ten);
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Two);
            machine.AcceptMoney(Denomination.Five);
            machine.AcceptMoney(Denomination.Ten);
            machine.AcceptMoney(Denomination.One);

            //Act
            var actual = machine.ReturnMoney();

            //Test
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.DenominiationCounts.Count);

            Assert.AreEqual(4, actual.DenominiationCounts[Denomination.One]);
            Assert.AreEqual(1, actual.DenominiationCounts[Denomination.Two]);
            Assert.AreEqual(2, actual.DenominiationCounts[Denomination.Five]);
            Assert.AreEqual(2, actual.DenominiationCounts[Denomination.Ten]);
        }

        [TestMethod]
        public void VendingMachine_ReturnMoney_UserInputtedMoneyIsResetToEmptyMoneyBag()
        {
            //Setup
            DefaultInitialization();
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Five);
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Ten);
            machine.AcceptMoney(Denomination.One);
            machine.AcceptMoney(Denomination.Two);
            machine.AcceptMoney(Denomination.Five);
            machine.AcceptMoney(Denomination.Ten);
            machine.AcceptMoney(Denomination.One);

            //Act
            machine.ReturnMoney();

            //Test
            Assert.IsNotNull(machine.UserInputtedMoney);
            var nonZeroValues = machine.UserInputtedMoney.DenominiationCounts.Where(z => z.Value != 0);
            Assert.AreEqual(0, nonZeroValues.Count());
        }
                
        #endregion

    }
}
