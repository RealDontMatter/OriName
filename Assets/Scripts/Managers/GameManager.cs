using Assets.Scripts.SO;
using Components;
using Models;
using NUnit.Framework;
using SO;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Models.Interfaces;
using System.Linq;

namespace Managers
{
    class GameManager : MonoBehaviour
    {

        public Views.InventoryView InventoryView;
        public Views.OverlayView OverlayView;

        public InterfaceMediator InterfaceMediator;
        public InventoryInterfaceController InventoryInterfaceController;
        public OverlayInterfaceController OverlayInterfaceController;
        public PlayerComponent PlayerController;
        public InteractablesSpawner InteractablesSpawner;

        public GameObject InteractablesParent;


        public Models.Settings.HomeSettings HomeSettings;
        public StartingItemsScriptableObject StartingItems;


        [SerializeField]
        private PlayerSettingsSO m_playerSettings;

        private Inventory m_inventory;


        private void Start()
        {
            m_inventory = new();
            foreach (var entry in StartingItems.Items)
            {
                m_inventory.AddItem(entry.Type, entry.Count);
            }


            PlayerController.Initialize(m_inventory);


            InteractablesSpawner.Initialize(HomeSettings, InteractablesParent.transform);
            List<DestroyableComponent> interactables = InteractablesSpawner.SpawnObjects();
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
