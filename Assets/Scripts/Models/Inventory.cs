using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    class Inventory
    {
        private List<Item> Items;
        
        public Inventory()
        {
            Items = new();
            for (int i = 0; i < 20; i++)
            {
                Items.Add(null);
            }
        }


        public List<Item> GetItems() => Items;
        public void SetItem(Item item, int index) => Items[index] = item;

    }
}
