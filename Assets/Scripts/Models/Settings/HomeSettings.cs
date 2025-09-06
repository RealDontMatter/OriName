using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models.Settings
{
    [CreateAssetMenu(menuName = "Models/Settings/Home Settings")]
    public class HomeSettings : ScriptableObject
    {
        public float InteractableMinimumRadius;
        public Vector3 InteractableMinimumPosition;
        public Vector3 InteractableMaximumPosition;
        public List<InteractablesSpawnInfo> StartingInteractables;

        [Serializable]
        public struct InteractablesSpawnInfo
        {
            public GameObject Prefab;
            public int Count;
        }
    }
}
