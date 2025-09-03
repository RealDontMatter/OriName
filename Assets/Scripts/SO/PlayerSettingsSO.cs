using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(menuName = "SO/PlayerSettings")]
    public class PlayerSettingsSO : ScriptableObject
    {
        public float MovementSpeed;
        public int InventorySize; // Not implemented yet
    }
}
