using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views
{
    class InventorySlot : MonoBehaviour, 
        IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public GameObject background, item, count, frame;
        public Image backgroundImage, itemImage;
        public Text countText;
        
        [HideInInspector] public CanvasGroup CanvasGroup;


        public Action Clicked;
        public event Action DragBegined, DragEnded;

        void Start()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            
        }

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

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemImage.transform.SetParent(transform.parent);
            itemImage.transform.SetAsLastSibling();
            countText.gameObject.SetActive(false);
            DragBegined?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            itemImage.transform.position = eventData.position;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            itemImage.transform.SetParent(transform);
            countText.transform.SetAsLastSibling();
            countText.gameObject.SetActive(Count > 1);

            var rectTransform = itemImage.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            DragEnded?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke();
    }
}