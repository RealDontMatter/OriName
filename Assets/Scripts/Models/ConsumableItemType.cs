using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Models/ConsumableItemType")]
    public class ConsumableItemType : ItemType
    {
        [SerializeField] private int m_hitPoints;

        public int HitPoints => m_hitPoints;

        public override Item CreateItem(int count = 1) => new ConsumableItem(this, count);
    }
}
