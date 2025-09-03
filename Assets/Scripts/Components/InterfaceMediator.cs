using UnityEngine;

namespace Components 
{
    class InterfaceMediator : MonoBehaviour
    {
        private bool isInitialized;
        public InventoryInterfaceController InventoryInterfaceController;
        public OverlayInterfaceController OverlayInterfaceController;

        public void Initialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                InventoryInterfaceController.Closed += () =>
                {
                    InventoryInterfaceController.SetActive(false);
                    OverlayInterfaceController.SetActive(true);
                };
                OverlayInterfaceController.InventoryOpened += () => 
                {
                    InventoryInterfaceController.SetActive();
                    OverlayInterfaceController.SetActive(false);
                };
            }
        }
    }
}
