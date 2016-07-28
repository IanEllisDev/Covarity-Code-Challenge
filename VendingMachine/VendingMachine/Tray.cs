using Covarity.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Covarity.Exceptions;

namespace Covarity
{
    public class Tray 
    {
        private const int MAX_ITEMS = 10;
        public int Price { get; private set; }
        public Stack<ItemBase> Items;

        public int Count
        {
            get { return Items == null ? 0 : Items.Count; }
        }

        public bool HasItems
        {
            get { return Count > 0; }
        }

                
        public Tray()
        {
            Items = new Stack<ItemBase>(10);
            Price = 0;
        }

        //Adds item to the beginning of the tray.
        public void AddItem(ItemBase item, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than 0");
            }
            if (item == null)
            {
                throw new ArgumentException("Item cannot be null");
            }
            
            //Items added to a tray with existing items much match in price
            if (HasItems && Items.Peek().Amount != item.Amount)
            {
                throw new InvalidTrayItemException("Item does not match the price of existing tray items");
            }

            //Items can't exceed the maximum amount of items.
            if (count + Items.Count > MAX_ITEMS)
            {
                throw new InvalidTrayItemException("Item count exceeds the maximum allowable items on the tray. Try adding fewer items or removing existing items from the tray.");
            }

            //Add items to the tray
            for (var i = 0; i < count; i++)
            {
                Items.Push(item);
            }
            Price = item.Amount;
        }

        public ItemBase RemoveItem()
        {
            return HasItems ? Items.Pop() : null;
        }
   
    }
}
