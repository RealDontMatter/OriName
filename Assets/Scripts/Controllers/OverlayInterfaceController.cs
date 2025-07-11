using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    class OverlayInterfaceController : MonoBehaviour
    {
        private bool isInitialized;
        public EventHandler InventoryOpened;
        
        [Header("View")]
        public GameObject View;
        public Button InventoryButton;


        public void Initialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                InventoryButton.onClick.AddListener(InventoryClick);
            }
        }

        private void InventoryClick()
        {
            InventoryOpened?.Invoke(this, EventArgs.Empty);
        }

        public void Open()
        {
            View.SetActive(true);
        }
        public void Close()
        {
            View.SetActive(false);
        }
    }
}
