using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Models.Interfaces;

namespace Models
{
    [Serializable]
    public class ConsumableItem : Item
    {
        private ConsumableItemType ConsumableData => (ConsumableItemType)m_itemType;

        private int m_hitPoints;

        public int HitPoints => m_hitPoints;
        public float HealthPercentage => (float)m_hitPoints / ConsumableData.HitPoints;

        // Initialization
        public ConsumableItem(ConsumableItemType type, int count) : base(type, count)
        {
            m_hitPoints = ConsumableData.HitPoints;
        }
        public ConsumableItem(ConsumableItem other) : base(other)
        {
            if (other is ConsumableItem consumable)
            {
                m_hitPoints = consumable.m_hitPoints;
            }
            else
            {
                throw new ArgumentException("The provided item is not a ConsumableItem.", nameof(other));
            }
        }
        public override object Clone() => new ConsumableItem(this);

        // Logic
        public bool CanConsume() => true;
        public void Consume()
        {
            m_hitPoints--;
            Changed?.Invoke();
            if (m_hitPoints <= 0)
            {
                m_hitPoints = ConsumableData.HitPoints;
                Remove(1);
            }
        }
    }
}
