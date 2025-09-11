using System;
using UnityEngine;
using UnityEngine.Events;

namespace Models
{
    [Serializable]
    public class Item : ICloneable
    {
        public Action<Item> Emptied;
        public UnityAction Changed;

        [SerializeField] protected ItemType m_itemType;
        [SerializeField] protected int m_count;

        public ItemType ItemType => m_itemType;
        public int Count => m_count;
        public int MaxStack => m_itemType.MaxStack;
        public bool IsFull => m_count >= MaxStack;
        public bool IsEmpty => m_count <= 0;

        // Initialization
        public Item(ItemType type, int count)
        {
            m_itemType = type;
            m_count = count;
        }
        public Item(Item other) : this(other.ItemType, other.Count) { }
        public virtual object Clone() => new Item(this);

        // Logic
        public (int added, int remaining) Add(int count)
        {
            if (m_count >= MaxStack)
                return (0, count);
            int spaceLeft = MaxStack - m_count;
            int toAdd = Math.Min(spaceLeft, count);
            m_count += toAdd;
            int remaining = count - toAdd;
            Changed?.Invoke();
            return (toAdd, remaining);
        }
        public (int removed, int remaining) Remove(int count)
        {
            int toRemove = Math.Min(m_count, count);
            int remainingToRemove = count - toRemove;
            m_count -= toRemove;
            if (m_count <= 0)
            {
                m_count = 0;
            }
            Emptied?.Invoke(this);
            Changed?.Invoke();
            return (toRemove, remainingToRemove);
        }
        public bool CanSplit() => m_count > 1;
        public (ItemType newItemType, int newItemCount) Split()
        {
            if (!CanSplit())
            {
                Debug.LogError("Cannot split an item with count less than 2.");
                return (null, 0);
            }

            int halfCount = m_count / 2;
            m_count -= halfCount;
            Changed?.Invoke();
            return (ItemType, halfCount);
        }
        public void FillFrom(Item other)
        {
            if (other == null || other.ItemType != this.ItemType)
            {
                Debug.LogWarning("Cannot fill from the provided item. (null or different type)");
                return; 
            }

            var (added, _) = Add(other.Count);
            other.Remove(added);
            Changed?.Invoke();
        }
    }
}
