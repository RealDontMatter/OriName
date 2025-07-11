using Controllers;
using Models;
using System;
using UnityEngine;

namespace Managers
{
    class GameManager : MonoBehaviour
    {
        public ItemsRegistry ItemsRegistry;
        public InterfaceMediator InterfaceMediator;
        public InventoryInterfaceController InventoryInterfaceController;
        public OverlayInterfaceController OverlayInterfaceController;
        public static GameManager Instance { get; private set; }
        public Inventory Inventory;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("A duplicate GameManager instance was found. Destroying it.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            ItemsRegistry.Initialize();
            LoadInventory();

            InitializeUI();
        }


        private void LoadInventory()
        {
            Inventory = new();

            var i0 = ItemsRegistry.Instance.GetItemByName("Wood").Clone() as Item;
            var i1 = ItemsRegistry.Instance.GetItemByName("Stone").Clone() as Item;

            i0.Count = 10;
            i1.Count = 7;

            Inventory.SetItem(i0, 0);
            Inventory.SetItem(i1, 1);
        }

        private void InitializeUI()
        {
            OverlayInterfaceController.Initialize();
            InventoryInterfaceController.Initialize(Inventory);
            InterfaceMediator.Initialize();
        }

    }
}
