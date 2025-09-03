using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Models/ItemType")]
    public class ItemType : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_image;
        [SerializeField] private int m_maxStack = 20;

        public string Name => m_name;
        public Sprite Image => m_image;
        public int MaxStack => m_maxStack;

        public Item CreateItem(int count = 1) => new Item(this, count);
    }
}
