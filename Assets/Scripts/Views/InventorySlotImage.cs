using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views
{
    public class InventorySlotImage : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Image m_image;
        private Transform m_parentAfterDrag, m_canvas;

        private void Start()
        {
            m_image = GetComponent<Image>();
            m_canvas = GameObject.Find("Canvas").transform;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            m_parentAfterDrag = transform.parent;


            transform.SetParent(m_canvas);
            transform.SetAsLastSibling();
            m_image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(m_parentAfterDrag);
            
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;

            m_image.raycastTarget = true;
        }
    }
}
