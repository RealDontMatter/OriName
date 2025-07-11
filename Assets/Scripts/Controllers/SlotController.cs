using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    class SlotController : MonoBehaviour
    {
        private bool isInitialized;

        public GameObject background, item, count, frame;
        public Image backgroundImage, itemImage;
        public Text countText;

        public void Initialize(Sprite itemSprite, int count, bool selected)
        {
            if (!isInitialized)
            {
                isInitialized = true;
            }

            this.ItemSprite = itemSprite;
            this.Count = count;
            this.IsSelected = selected;
        }
        public int Count
        {
            set
            {
                this.countText.text = $"{value}";
                this.count.SetActive(value > 1);
            }
        }
        public Sprite ItemSprite
        {
            set
            {
                this.item.SetActive(value != null);
                this.itemImage.sprite = value;
            }
        }
        public bool IsSelected
        {
            set
            {
                this.frame.SetActive(value);
            }
        }
    }
}