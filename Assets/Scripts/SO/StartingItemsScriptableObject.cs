using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "SO/StartingItems")]
    public class StartingItemsScriptableObject : ScriptableObject
    {
        public List<ItemEntry> Items;

        [Serializable] public struct ItemEntry
        {
            public Models.ItemType Type;
            public int Count;
        }
    }
}
