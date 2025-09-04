using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utility;
using Debug = UnityEngine.Debug;

namespace Models
{
    public class Inventory
    {
        public event Action ItemsChanged;

        public List<Item> m_items;
        
        public Inventory(int size = 20)
        {
            m_items = Enumerable.Repeat<Item>(null, size).ToList();
        }


        public List<Item> Items => m_items;
        public bool IsFull => !m_items.Any(i => i == null || !i.IsFull );
        public int Size => m_items.Count;
        public bool HasEmptySlots => m_items.Any(i => i == null);


        public void SetItem(int index, Item item) 
        {
            if (!checkIndex(index)) 
            {
                Debug.LogError($"Inventory.SetItem: Invalid index '{index}', Inventory Size {m_items.Count}");
                return;
            }
            m_items[index] = item;

            ItemsChanged?.Invoke();
        }
        

        public (int added, int remaining) AddItem(ItemType type, int Count)
        {
            int startCount = Count;

            // fill existing slots
            Item findItem = m_items.Find(i => i != null && i.ItemType == type && !i.IsFull);
            while (findItem != null && Count > 0) {
                var (added, remaining) = findItem.Add(Count);
                Count = remaining;

                findItem = m_items.Find(i => i != null && i.ItemType == type && !i.IsFull);
            }

            // Continue only if items left
            if (Count <= 0) { 
                ItemsChanged?.Invoke();
                return (startCount, 0); 
            }

            // Fill empty slots
            int emptyIndex = m_items.IndexOf(null);
            while ( emptyIndex != -1 && Count > 0)
            {
                Item emptyItem = new Item(type, 0);
                var (added, remaining) = emptyItem.Add(Count);
                Count = remaining;

                m_items[emptyIndex] = emptyItem;

                emptyIndex = m_items.IndexOf(null);
            }
            ItemsChanged?.Invoke();
            return (Count, startCount - Count);
        }
        public (int added, int remaining) AddItem(Item item) => AddItem(item.ItemType, item.Count);
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
        
        public void SplitItem(int index)
        {
            if (!checkIndex(index))
            {
                Debug.LogError($"Inventory.SplitItem: Invalid index '{index}', Inventory Size {m_items.Count}"); 
                return;
            }
            if (!HasEmptySlots) 
            {
                Debug.LogWarning($"Inventory.SplitItem: No empty slots available");
                return;
            }

            Item givenSlotItem = m_items[index];

            int emptySlotIndex = m_items.IndexOf(null);
            Item emptySlotItem = givenSlotItem.ItemType.CreateItem((givenSlotItem.Count + 1) / 2);
            m_items[emptySlotIndex] = emptySlotItem;

            givenSlotItem.Count /= 2;

            ItemsChanged?.Invoke();
        }
        public void SwapItems(int fromIndex, int toIndex)
        {
            if(!checkIndex(fromIndex))
            {
                Debug.LogError($"Inventory.SwapItems: Invalid fromIndex '{fromIndex}', Inventory Size {m_items.Count}");
                return;
            }
            if (!checkIndex(toIndex))
            {
                Debug.LogError($"Inventory.SwapItems: Invalid toIndex '{toIndex}', Inventory Size {m_items.Count}");
                return;
            }

            var fromItem = m_items[fromIndex];
            var toItem = m_items[toIndex];

            if (fromItem != null && toItem != null && fromItem.ItemType == toItem.ItemType)
            {
                var (_, remaining) = toItem.Add(fromItem.Count);
                fromItem.Count = remaining;
                if (fromItem.Count <= 0) m_items[fromIndex] = null;
            }
            else
            {
                m_items[fromIndex] = toItem;
                m_items[toIndex] = fromItem;
            }


            ItemsChanged?.Invoke();
        }
        private bool checkIndex(int index) => index >= 0 && index < m_items.Count;

        // TODO---------------
        public void Sort() 
        {
            m_items.Sort((i1, i2) => i1.ItemType.Name.CompareTo(i2.ItemType.Name));
            ItemsChanged?.Invoke();
        }
        //---------------------
    }
}
