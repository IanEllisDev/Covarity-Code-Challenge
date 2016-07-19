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
            Assert.AreEqual(0, machine.Money.DenominiationCounts.Where(z => z.Value > 0).Count());
            Assert.AreEqual(0, machine.UserInputtedMoney.DenominiationCounts.Where(z => z.Value > 0).Count());
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
            Assert.AreEqual(3, machine.Money.DenominiationCounts.Where(z => z.Value > 0).Count());
            Assert.AreEqual(0, machine.UserInputtedMoney.DenominiationCounts.Where(z => z.Value > 0).Count());

            var denominizations = machine.Money.DenominiationCounts;
            Assert.IsTrue(denominizations.ContainsKey(Denomination.One));
            Assert.IsTrue(denominizations.ContainsKey(Denomination.Two));
            Assert.IsTrue(denominizations.ContainsKey(Denomination.Five));
            Assert.IsTrue(denominizations.ContainsKey(Denomination.Ten));
            Assert.AreEqual(ones, denominizations[Denomination.One]);
            Assert.AreEqual(0, denominizations[Denomination.Two]);
            Assert.AreEqual(fives, denominizations[Denomination.Five]);
            Assert.AreEqual(tens, denominizations[Denomination.Ten]);

        }

        #endregion

        #region Accept Money
                
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
            Assert.AreEqual(0, actual.DenominiationCounts.Where(z => z.Value > 0).Count());
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
            Assert.AreEqual(1, actual.DenominiationCounts.Where(z => z.Value > 0).Count());
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

        #region Return Change

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void VendingMachine_ReturnChange_NoMoneyInMachine_ExceptionOccurred()
        {
            machine = new VendingMachine(null);
            machine.ReturnChange(500);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void VendingMachine_ReturnChange_ReturnNegativeAmount_ExceptionOccurred()
        {
            machine = new VendingMachine(null);
            machine.ReturnChange(-50);
        }

        [TestMethod]
        public void VendingMachine_ReturnChange_NoChangeToReturn_ReturnEmptyMoneyBag()
        {
            machine = new VendingMachine(null);
            var actual = machine.ReturnChange(0);
            Assert.AreEqual(0, actual.Amount);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void VendingMachine_ReturnChange_CannotReturnCorrectChangeInsufficentOnes_ExceptionOccurred()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 0 },
                { Denomination.Two, 100000 },
                { Denomination.Five, 100000 },
                { Denomination.Ten, 100000 },
            });

            machine.ReturnChange(3); //Five/Tens are too much, and no ones to make 3$
            Assert.Fail("Failed to respond to impossible change return with an exception");
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void VendingMachine_ReturnChange_CannotReturnCorrectChangeInsufficentOnesAndTwos_ExceptionOccurred()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 2 },
                { Denomination.Two, 0 },
                { Denomination.Five, 100000 },
                { Denomination.Ten, 100000 },
            });

            machine.ReturnChange(3); //Five/Tens are too much, and not enough ones/twos to make 3$
            Assert.Fail("Failed to respond to impossible change return with an exception");
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void VendingMachine_ReturnChange_CannotReturnCorrectChangeInsufficentMoney_ExceptionOccurred()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 2 },
                { Denomination.Two, 0 },
                { Denomination.Five, 1 },
                { Denomination.Ten, 0 },
            });

            machine.ReturnChange(10); //Not enough money in the machine to make change with;
            Assert.Fail("Failed to respond to impossible change return with an exception");
        }

        [TestMethod]
        public void VendingMachine_ReturnChange_SingleOne_ChangeReturned()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 1 },
                { Denomination.Two, 0 },
                { Denomination.Five, 0 },
                { Denomination.Ten, 0 },
            });

            var actual = machine.ReturnChange(1);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Amount, 1);
        }

        [TestMethod]
        public void VendingMachine_ReturnChange_SingleTwo_ChangeReturned()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 0 },
                { Denomination.Two, 1 },
                { Denomination.Five, 0 },
                { Denomination.Ten, 0 },
            });

            var actual = machine.ReturnChange(2);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Amount, 2);
        }

        [TestMethod]
        public void VendingMachine_ReturnChange_SingleFive_ChangeReturned()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 0 },
                { Denomination.Two, 0 },
                { Denomination.Five, 1 },
                { Denomination.Ten, 0 },
            });

            var actual = machine.ReturnChange(5);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Amount, 5);
        }

        [TestMethod]
        public void VendingMachine_ReturnChange_SingleTen_ChangeReturned()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 0 },
                { Denomination.Two, 0 },
                { Denomination.Five, 0 },
                { Denomination.Ten, 1 },
            });

            var actual = machine.ReturnChange(10);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Amount, 10);
        }

        [TestMethod]
        public void VendingMachine_ReturnChange_NoTens_ChangeReturnedUsingSmallerAmounts()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 2 },
                { Denomination.Two, 4 },
                { Denomination.Five, 5 },
                { Denomination.Ten, 0 },
            });

            var actual = machine.ReturnChange(23);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Amount, 23);
            Assert.IsFalse(actual.DenominiationCounts.ContainsKey(Denomination.Ten) && actual.DenominiationCounts[Denomination.Ten] > 0);
        }


        [TestMethod]
        public void VendingMachine_ReturnChange_TrickyChangeScenario_ChangeReturnedUsingSmallerAmounts()
        {
            //If return change always tries to return highest value, it will get to 35 and won't have the ones to process 40
            //However, if it only takes 2 of the fives, there are enough twos to make the correct change
            //Likewise, if it tries to take all of the Twos, you'll get a number ending in '4' and won't be able to turn it into a 0
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 0 },
                { Denomination.Two, 7 },
                { Denomination.Five, 3 },
                { Denomination.Ten, 2 },
            });

            var actual = machine.ReturnChange(40);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Amount, 40);
        }

        [TestMethod]
        public void VendingMachine_ReturnChange_LotsOfCoinsAvailable_ChangeReturned()
        {
            machine = new VendingMachine(new Dictionary<Denomination, int>()
            {
                { Denomination.One, 500000 },
                { Denomination.Two, 500000 },
                { Denomination.Five, 500000 },
                { Denomination.Ten, 500000 },
            });

            var actual = machine.ReturnChange(241);
            Assert.IsNotNull(actual);
            Assert.AreEqual(241, actual.Amount);
        }

        [TestMethod]
        public void VendingMachine_ReturnChange_ExactAmountReturned_ChangeReturned()
        {
            var ones = 1;
            var twos = 2;
            var fives = 3;
            var tens = 4;

            var expected = new Dictionary<Denomination, int>()
            {
                { Denomination.One, ones },     //  1
                { Denomination.Two, twos },     //  4
                { Denomination.Five, fives },   // 15
                { Denomination.Ten, tens },     // 40  
                                                //=60
            };
            var expectedAmount = 60;
            machine = new VendingMachine(expected);

            var actual = machine.ReturnChange(60);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedAmount, actual.Amount);
            Assert.AreEqual(ones, actual.DenominiationCounts[Denomination.One]);
            Assert.AreEqual(twos, actual.DenominiationCounts[Denomination.Two]);
            Assert.AreEqual(fives, actual.DenominiationCounts[Denomination.Five]);
            Assert.AreEqual(tens, actual.DenominiationCounts[Denomination.Ten]);


        }

        #endregion

    }
}
