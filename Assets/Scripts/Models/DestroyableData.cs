using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Models/DestroyableData")]
    public class DestroyableData : InteractableData
    {
        [SerializeField] private List<Item> m_itemsToDrop, m_requiredItems;
        [SerializeField] private int m_totalHitPoints;
        [SerializeField] private float m_hitDuration;

        public IEnumerable<Item> ItemsToDrop => m_itemsToDrop;
        public IEnumerable<Item> RequiredItems => m_requiredItems;
        public int TotalHitPoints => m_totalHitPoints;
        public float HitDuration => m_hitDuration;

        public Destroyable CreateDestroyable() => new Destroyable(this);
    }
}
