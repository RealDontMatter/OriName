using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Models/DestroyableData")]
    public class DestroyableType : InteractableType
    {
        [SerializeField] private List<Item> m_itemsToDrop;
        [SerializeField] private int m_totalHitPoints;
        [SerializeField] private float m_hitDuration;
        [SerializeField] private ConsumableItemType m_toolType;

        public IEnumerable<Item> ItemsToDrop => m_itemsToDrop;
        public ConsumableItemType ToolType => m_toolType;
        public int TotalHitPoints => m_totalHitPoints;
        public float HitDuration => m_hitDuration;

        public override Interactable CreateInteractable() => new Destroyable(this);
    }
}
