using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views
{
    class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        public GameObject background, item, count, frame;
        public Image backgroundImage, itemImage;
        public Text countText;
        
        public Action Clicked;
        public Action<PointerEventData> Dropped;

        public int Count
        {
            get => int.Parse(this.countText.text);
            set
            {
                this.countText.text = $"{value}";
                this.count.SetActive(value > 1);
            }
        }
        public Sprite ItemSprite
        {
            get => this.itemImage.sprite;
            set
            {
                this.item.SetActive(value != null);
                this.itemImage.sprite = value;
            }
        }
        public bool IsSelected
        {
            get => this.frame.activeSelf;
            set => this.frame.SetActive(value);
        }

        public void OnDrop(PointerEventData eventData) => Dropped?.Invoke(eventData);
        public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke();
    }
}