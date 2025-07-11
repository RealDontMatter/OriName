using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    class Item : ICloneable
    {
        public string Name { get; set; }
        public Sprite Image { get; set; }
        public int Count { get; set; }

        public Item(string name, Sprite image, int count)
        {
            Name = name;
            Image = image;
            Count = count;
        }
        public Item(Item other)
        {
            Name = other.Name;
            Image = other.Image;
            Count = other.Count;
        }
        public object Clone()
        {
            return new Item(this);
        }
    }
}
