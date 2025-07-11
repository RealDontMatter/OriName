using Controllers;
using UnityEngine;


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
            InventoryInterfaceController.Closed += (sender, e) =>
            {
                InventoryInterfaceController.Close();
                OverlayInterfaceController.Open();
            };
            OverlayInterfaceController.InventoryOpened += (sender, e) => 
            {
                InventoryInterfaceController.Open();
                OverlayInterfaceController.Close();
            };
        }
    }

}
