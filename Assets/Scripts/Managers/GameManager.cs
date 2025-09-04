using Assets.Scripts.SO;
using Components;
using Models;
using NUnit.Framework;
using SO;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Managers
{
    class GameManager : MonoBehaviour
    {

        public Views.InventoryView InventoryView;
        public Views.OverlayView OverlayView;

        public InterfaceMediator InterfaceMediator;
        public InventoryInterfaceController InventoryInterfaceController;
        public OverlayInterfaceController OverlayInterfaceController;
        public PlayerController PlayerController;
        public InteractablesSpawner InteractablesSpawner;


        public HomeSettingsSO HomeSettingsSO;
        public StartingItemsScriptableObject StartingItems;


        [SerializeField]
        private PlayerSettingsSO m_playerSettings;

        private Inventory m_inventory;


        private void Start()
        {
            m_inventory = new();
            m_inventory.AddItems(StartingItems.Items.DeepClone());


            PlayerController.Initialize(m_inventory);

            List<IInteractable> interactables = InteractablesSpawner.SpawnObjects();
            interactables.ForEach((interactable) => { 
                interactable.Initialize(PlayerController.gameObject);
                PlayerController.RegisterInteractable(interactable); 
            });

            OverlayView.Initialize();
            InventoryView.Initialize(m_inventory.Size);

            OverlayInterfaceController.Initialize(PlayerController);
            InventoryInterfaceController.Initialize(m_inventory, InventoryView);
            InterfaceMediator.Initialize();
        }
    }
}
