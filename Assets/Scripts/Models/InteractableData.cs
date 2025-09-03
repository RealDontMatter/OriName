using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    public class InteractableData : ScriptableObject
    {
        [SerializeField] private GameObject m_prefab;

        public GameObject Prefab => m_prefab;
    }
}
