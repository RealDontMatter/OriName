using Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views
{
    public class InventoryView : MonoBehaviour, IPointerClickHandler, IDropHandler
    {
        private List<InventorySlot> m_slots = new();

        [SerializeField]
        private GameObject m_view, m_itemsHolder;
        [SerializeField]
        private Button m_splitButton, m_deleteButton, m_exitButton;
        [Header("Prefabs")]
        [SerializeField]
        private GameObject m_slotPrefab;


        public void Initialize(int itemsCount)
        {
            m_splitButton.onClick.AddListener(() => SplitClicked?.Invoke());
            m_deleteButton.onClick.AddListener(() => DeleteClicked?.Invoke());
            m_exitButton.onClick.AddListener(() => ExitClicked?.Invoke());

            for (int i = 0; i < itemsCount; i++)
            {
                int x = i % 5;
                int y = i / 5;

                var slot = Instantiate(m_slotPrefab, m_itemsHolder.transform);
                var slotTransform = slot.GetComponent<RectTransform>();
                slotTransform.anchoredPosition = new Vector2(68 * x, -68 * y);
                var slotController = slot.GetComponent<InventorySlot>();
                m_slots.Add(slotController);
            }

            for(int i = 0; i < m_slots.Count; i++)
            {
                int index = i; // Capture index for the lambda
                var slot = m_slots[i];

                slot.Clicked += () => SlotClicked?.Invoke(index);
                slot.DragBegined += () =>
                {
                    for (int j = 0; j < m_slots.Count; j++)
                    {
                        m_slots[j].CanvasGroup.blocksRaycasts = false;
                    }
                };
                slot.DragEnded += () =>
                {
                    for (int j = 0; j < m_slots.Count; j++)
                    {
                        m_slots[j].CanvasGroup.blocksRaycasts = true;
                    }
                };
            }

        }


        // Events
        public event Action<int> SlotClicked;
        public event Action SplitClicked, DeleteClicked, ExitClicked;
        public event Action<int, int> ItemDragged; // fromIndex, toIndex

        // General
        public void SetActive(bool active = true) => m_view.SetActive(active);
        public bool IsActive() => m_view.activeInHierarchy;
        // Items
        public void UpdateItems(List<Item> items)
        {
            for (int i = 0; i < items.Count && i < m_slots.Count; i++)
            {
                var item = items[i];
                var slot = m_slots[i];

                slot.ItemSprite = item?.ItemType.Image;
                slot.Count = item?.Count ?? 0;

                if (item is ConsumableItem consumable)
                    slot.Duration = consumable.HealthPercentage;
                else
                    slot.Duration = 0;
            }
        }

        public void UpdateItem(int index, Item item)
        {
            if (index < 0 || index >= m_slots.Count) return;
            var slot = m_slots[index];
            slot.ItemSprite = item?.ItemType.Image;
            slot.Count = item?.Count ?? 0;
            if (item is ConsumableItem consumable)
                slot.Duration = consumable.HealthPercentage ;
            else
                slot.Duration = 0;
        }

        public void SelectItem(int index, bool selected = true)
        {
            if ( index < 0 || index >= m_slots.Count) return;
            for (int i = 0; i < m_slots.Count; i++)
            {
                bool select = i == index && selected;
                m_slots[i].IsSelected = select;
            }
        }
        public void DeselectAllItems()
        {
            for (int i = 0; i < m_slots.Count; i++)
            {
                m_slots[i].IsSelected = false;
            }
        }
        // Split Button
        public void SplitButtonSetActive(bool active = true) => m_splitButton.interactable = active;
        // Delete Button
        public void DeleteButtonSetActive(bool active = true) => m_deleteButton.interactable = active;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on InventoryView background" + eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var slot = eventData.pointerDrag.GetComponent<InventorySlot>();

            int startIndex = m_slots.IndexOf(slot);
            int endIndex = -1;

            var mousePosition = eventData.position;

            for (int i = 0; i < m_slots.Count; i++)
            {
                var rect = m_slots[i].GetComponent<RectTransform>();
                if(RectTransformUtility.RectangleContainsScreenPoint(rect, mousePosition))
                {
                    endIndex = i;
                    break;
                }
            }

            if (startIndex != endIndex &&  
                startIndex < m_slots.Count && startIndex >= 0 && 
                endIndex < m_slots.Count && endIndex >= 0)
            {
                ItemDragged?.Invoke(startIndex, endIndex);
            }
        }
    }
}
