using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Player
    {
        private Inventory m_inventory;

        public Inventory Inventory => m_inventory;

        public bool HasItem(ItemType type) => m_inventory.HasItem(type);
        public void AddItem(ItemType type, int count = 1) => m_inventory.AddItem(type, count);
        public void RemoveItem(ItemType type, int count = 1) => m_inventory.RemoveItem(type, count);

        public Player()
        {
            m_inventory = new Inventory();
        }
    }
}
