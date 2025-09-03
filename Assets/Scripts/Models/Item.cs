using SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Item : ICloneable
    {
        [SerializeField] private ItemType m_itemType;
        [SerializeField] private int m_count;

        public ItemType ItemType => m_itemType;
        public int Count { get => m_count; set => m_count = value; }
        public int MaxStack => m_itemType.MaxStack;
        public bool IsFull => m_count >= MaxStack;
        public Item(ItemType type, int count)
        {
            m_itemType = type;
            m_count = count;
        }

        public (int added, int remaining) Add(int countToAdd)
        {
            if (m_count >= MaxStack)
                return (0, countToAdd);
            int spaceLeft = MaxStack - m_count;
            int toAdd = Math.Min(spaceLeft, countToAdd);
            m_count += toAdd;
            int remaining = countToAdd - toAdd;
            return (toAdd, remaining);
        }
        public (int added, int remaining) Add(Item itemToAdd)
        {
            if (itemToAdd.ItemType != ItemType)
                return (0, itemToAdd.Count);
            return Add(itemToAdd.Count);
        }







        public Item(Item other) : this(other.ItemType, other.Count) { }
        public object Clone() => new Item(this);
    }
}
