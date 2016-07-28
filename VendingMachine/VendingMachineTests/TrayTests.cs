using Covarity;
using Covarity.Exceptions;
using Covarity.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarityTests
{
    [TestClass]
    public class TrayTests
    {
        [TestMethod]
        public void Tray_Initialize()
        {
            var tray = new Tray();

            Assert.IsNotNull(tray);
            Assert.IsNotNull(tray.Items);
            Assert.AreEqual(0, tray.Count);
            Assert.AreEqual(0, tray.Price);
            Assert.IsFalse(tray.HasItems);
        }

        [TestMethod]
        public void Tray_AddItems_CountIsUpdated()
        {
            var expected = 7;
            var tray = new Tray();

            tray.AddItem(new AppleItem(), expected);

            Assert.AreEqual(expected, tray.Count);
        }

        [TestMethod]
        public void Tray_AddItems_TrayNowHasItems()
        {
            var count = 7;
            var tray = new Tray();

            tray.AddItem(new AppleItem(), count);

            Assert.IsTrue(tray.HasItems);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Tray_AddZeroItems_ArgumentExceptionOccurrs()
        {
            var tray = new Tray();

            tray.AddItem(new AppleItem(), 0);

            Assert.Fail("Exception not thrown");
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Tray_AddNullItem_ArgumentExceptionOccurrs()
        {
            var tray = new Tray();

            tray.AddItem(null, 5);

            Assert.Fail("Exception not thrown");
        }
        
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Tray_AddNullItemZeroTimes_ArgumentExceptionOccurrs()
        {
            var tray = new Tray();

            tray.AddItem(null, 0);

            Assert.Fail("Exception not thrown");
        }

        [ExpectedException(typeof(InvalidTrayItemException))]
        [TestMethod]
        public void Tray_AddItemWithDifferentPrice_InvalidTrayExceptionOccurrs()
        {
            var tray = new Tray();

            tray.AddItem(new AppleItem(), 1);
            tray.AddItem(new PeanutItem(), 1);

            Assert.Fail("Exception not thrown");
        }

        [ExpectedException(typeof(InvalidTrayItemException))]
        [TestMethod]
        public void Tray_AddTooManyItems_InvalidTrayExceptionOccurrs()
        {
            var tray = new Tray();

            tray.AddItem(new BagOfChipsItem(), 50);

            Assert.Fail("Exception not thrown");
        }

        [ExpectedException(typeof(InvalidTrayItemException))]
        [TestMethod]
        public void Tray_AddTooManyItemsToExistingTray_InvalidTrayExceptionOccurrs()
        {
            var tray = new Tray();

            tray.AddItem(new BagOfChipsItem(), 5);
            tray.AddItem(new BagOfChipsItem(), 7);

            Assert.Fail("Exception not thrown");
        }

        [TestMethod]
        public void Tray_AddItemsToExistingTrayThatFitProperly_AllItemsAdded()
        {
            var tray = new Tray();
            tray.AddItem(new AppleItem(), 2);
            tray.AddItem(new AppleItem(), 8);

            Assert.AreEqual(10, tray.Items.Count);
            Assert.AreEqual(new AppleItem().Amount, tray.Price);
        }

        [TestMethod]
        public void Tray_AddDifferentItemsWithSamePriceToExistingTrayThatFitProperly_AllItemsAdded_NewItemsAppearFirst()
        {
            var tray = new Tray();
            tray.AddItem(new BagOfChipsItem(), 2);
            tray.AddItem(new CookieItem(), 8);

            Assert.AreEqual(10, tray.Items.Count);
            Assert.AreEqual(new CookieItem().Amount, tray.Price);
            Assert.AreEqual(new CookieItem().Name, tray.Items.Peek().Name);
        }
    }
}
