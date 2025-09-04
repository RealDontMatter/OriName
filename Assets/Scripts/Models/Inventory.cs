using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models
{
    public class Inventory
    {
        public event Action ItemsChanged;

        public List<Item> Items;
        
        public Inventory(int size = 20)
        {
            Items = Enumerable.Repeat<Item>(null, size).ToList();
        }

        /// <summary>READ-ONLY (Deep Clone)</summary>
        public List<Item> GetItems() => Items;

        public bool IsFull => !Items.Any(i => i == null || !i.IsFull );
        public int Size => Items.Count;
        public bool HasEmptySlots => Items.Any(i => i == null);


        /// <returns>replacedItem</returns>
        public Item SetItem(Item item, int index) 
        {
            if (!checkIndex(index)) return null;
            Item old = Items[index];
            Items[index] = item;

            ItemsChanged?.Invoke();

            return old;
        }
        /// <returns>replacedItem</returns>
        public Item SetItem(ItemType type, int count, int index) => SetItem(type.CreateItem(count), index);
       
        public (Item removed, bool success) RemoveItem(int index)
        {
            if (!checkIndex(index)) return (null, false);
            Item old = Items[index];
            Items[index] = null;

            ItemsChanged?.Invoke();

            return (old, true);
        }

        public (int added, int remaining) AddItem(ItemType type, int Count)
        {
            int startCount = Count;

            // fill existing slots
            Item findItem = Items.Find(i => i != null && i.ItemType == type && !i.IsFull);
            while (findItem != null && Count > 0) {
                var (added, remaining) = findItem.Add(Count);
                Count = remaining;

                findItem = Items.Find(i => i != null && i.ItemType == type && !i.IsFull);
            }

            // Continue only if items left
            if (Count <= 0) { 
                ItemsChanged?.Invoke();
                return (startCount, 0); 
            }

            // Fill empty slots
            int emptyIndex = Items.IndexOf(null);
            while ( emptyIndex != -1 && Count > 0)
            {
                Item emptyItem = new Item(type, 0);
                var (added, remaining) = emptyItem.Add(Count);
                Count = remaining;

                Items[emptyIndex] = emptyItem;

                emptyIndex = Items.IndexOf(null);
            }
            ItemsChanged?.Invoke();
            return (Count, startCount - Count);
        }
        public (int added, int remaining) AddItem(Item item) => AddItem(item.ItemType, item.Count);

        /// <returns>Remaining Items</returns>
        public List<Item> AddItems(IEnumerable<Item> items)
        {
            List<Item> list = new();
            foreach (var item in items)
            {
                var (_, remaining) = AddItem(item);
                if(remaining > 0) list.Add(new Item(item.ItemType, remaining));
            }
            ItemsChanged?.Invoke();
            return list;
        }
        
        /// <param name="item">Item that is present in Inventory, NOT COPY</param>
        public void SplitItem(Item item) => SplitItem(Items.IndexOf(item));
        public void SplitItem(int index)
        {
            if (index < 0 || index >= Items.Count || !HasEmptySlots) return ;

            Item givenSlotItem = Items[index];

            int emptySlotIndex = Items.IndexOf(null);
            Item emptySlotItem = givenSlotItem.ItemType.CreateItem((givenSlotItem.Count + 1) / 2);
            Items[emptySlotIndex] = emptySlotItem;

            givenSlotItem.Count /= 2;

            ItemsChanged?.Invoke();
        }

        public void SwapItems(int fromIndex, int toIndex)
        {
            if(fromIndex < 0 || fromIndex >= Items.Count) return;
            if(toIndex <  0 || toIndex >= Items.Count) return;

            var fromItem = Items[fromIndex];
            var toItem = Items[toIndex];

            if (fromItem != null && toItem != null && fromItem.ItemType == toItem.ItemType)
            {
                var (_, remaining) = toItem.Add(fromItem.Count);
                fromItem.Count = remaining;
                if (fromItem.Count <= 0) Items[fromIndex] = null;
            }
            else
            {
                Items[fromIndex] = toItem;
                Items[toIndex] = fromItem;
            }


            ItemsChanged?.Invoke();
        }


        public void Sort() 
        {
            Items.Sort((i1, i2) => i1.ItemType.Name.CompareTo(i2.ItemType.Name));
            ItemsChanged?.Invoke();
        }

        private bool checkIndex(int index) => index >= 0 && index < Items.Count;

    }
}
