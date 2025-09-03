using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "SO/HomeSettings")]
    public class HomeSettingsSO : ScriptableObject
    {
        // --------- Interactables --------------
        public float InteractableMinimumRadius;
        public Vector3 InteractableMinimumPosition;
        public Vector3 InteractableMaximumPosition;
        public List<InteractablesSpawnInfo> StartingInteractables;

        [Serializable]
        public struct InteractablesSpawnInfo
        {
            public Models.InteractableData ToSpawn;
            public int Count;
        }
    }
}
