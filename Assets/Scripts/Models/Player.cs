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

        public Player()
        {
            m_inventory = new Inventory();
        }
    }
}
