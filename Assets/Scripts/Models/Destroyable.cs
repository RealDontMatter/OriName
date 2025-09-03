using SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utility;

namespace Models
{

    public class Destroyable
    {
        private int m_hitPoints;
        private DestroyableData m_data;

        public int HitPoints { get => m_hitPoints; set => m_hitPoints = value; }
        public DestroyableData Data => m_data;

        public Destroyable(DestroyableData data)
        {
            m_data = data;
            m_hitPoints = data.TotalHitPoints;
        }

    }
}
