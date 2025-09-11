using Components;
using SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace Models
{

    public class Destroyable : Interactable
    {
        public UnityAction Destroyed;
        private int m_hitPoints;

        public DestroyableType DestroyableType => (DestroyableType)m_type;
        public int HitPoints { get => m_hitPoints; set => m_hitPoints = value; }

        public Destroyable(DestroyableType data)
        {
            m_type = data;
            m_hitPoints = data.TotalHitPoints;
        }

        public void Hit()
        {
            m_hitPoints--;
            if (m_hitPoints <= 0)
            {
                Destroyed?.Invoke();
            }
        }
        public bool CanInteract (Inventory playerInventory) => DestroyableType.ToolType == null || playerInventory.HasItem(DestroyableType.ToolType);

    }
}
