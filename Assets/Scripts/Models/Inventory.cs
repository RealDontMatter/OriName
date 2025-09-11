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

            item.Emptied -= OnItemEmptied;
            item.Emptied += OnItemEmptied;


            ItemsChanged?.Invoke();
        }

        private void OnItemEmptied(Item item)
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(int index)
        {
            if (!checkIndex(index))
            {
                Debug.LogError($"Inventory.RemoveItem: Invalid index '{index}', Inventory Size {m_items.Count}");
                return;
            }

            if (m_items != null)
            {
                m_items[index].Emptied -= OnItemEmptied;
            }
            m_items[index] = null;
            
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
                Item emptyItem = type.CreateItem(0);
                emptyItem.Emptied -= OnItemEmptied;
                emptyItem.Emptied += OnItemEmptied;
                var (added, remaining) = emptyItem.Add(Count);
                Count = remaining;

                m_items[emptyIndex] = emptyItem;

                emptyIndex = m_items.IndexOf(null);
            }
            ItemsChanged?.Invoke();
            return (Count, startCount - Count);
        }
        public (int added, int remaining) AddItem(Item item) => AddItem(item.ItemType, item.Count);
        public void AddItems(IEnumerable<Item> items)
        {
            foreach (var item in items)
                AddItem(item);
            ItemsChanged?.Invoke();
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

            if (givenSlotItem == null)
            {
                Debug.LogWarning($"Inventory.SplitItem: No item in the given slot {index}");
                return;
            }
            if (!givenSlotItem.CanSplit())
            {
                Debug.LogWarning($"Inventory.SplitItem: Item in the given slot {index} cannot be split");
                return;
            }

            int emptySlotIndex = m_items.IndexOf(null);
            Items[emptySlotIndex] = givenSlotItem.Split();
            Items[emptySlotIndex].Emptied -= OnItemEmptied;
            Items[emptySlotIndex].Emptied += OnItemEmptied;
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
                toItem.FillFrom(fromItem);
                if (fromItem.IsEmpty) m_items[fromIndex] = null;
            }
            else
            {
                m_items[fromIndex] = toItem;
                m_items[toIndex] = fromItem;
            }

            ItemsChanged?.Invoke();
        }
        public bool HasItem(ItemType type) => m_items.Any(i => i != null && i.ItemType == type);
        public Item GetItem(ItemType type) => m_items.FirstOrDefault(i => i != null && i.ItemType == type);

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
