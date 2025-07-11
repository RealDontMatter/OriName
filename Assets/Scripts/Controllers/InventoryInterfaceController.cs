using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    class InventoryInterfaceController : MonoBehaviour
    {
        private bool isInitialized;
        private Inventory Inventory;
        public EventHandler Closed;

        [Header("View")]
        public GameObject View;
        public GameObject Items;
        public Button ExitButton;

        [Header("Prefabs")]
        public GameObject SlotPrefab;


        public void Initialize(Inventory inventory)
        {
            if(!isInitialized)
            {
                isInitialized = true;
                Inventory = inventory;
                ExitButton.onClick.AddListener(() => { Closed?.Invoke(this, EventArgs.Empty); });
            }

        }
        public void Open()
        {
            foreach(Transform item in Items.transform)
            {
                Destroy(item.gameObject);
            }

            for(int i = 0; i < 20; i++)
            {
                int x = i % 5;
                int y = i / 5;

                var slot = Instantiate(SlotPrefab, Items.transform);
                var slotTransform = slot.GetComponent<RectTransform>();
                slotTransform.anchoredPosition = new Vector2(68 * x, -68 * y);
                var slotController = slot.GetComponent<SlotController>();

                var item = Inventory.GetItems()[i];
                Sprite sprite = item == null ? null : item.Image;
                int count = item == null ? 0 : item.Count;
                slotController.Initialize(sprite, count, false);
            }

            View.SetActive(true);
        }
        public void Close()
        {
            View.SetActive(false);
        }
    }
}
