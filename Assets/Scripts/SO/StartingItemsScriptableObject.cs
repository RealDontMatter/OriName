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
        public List<Models.Item> Items;
    }
}
