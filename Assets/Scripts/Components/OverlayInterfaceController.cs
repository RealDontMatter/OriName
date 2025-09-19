using System;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace Components
{
    class OverlayInterfaceController : MonoBehaviour
    {
        private PlayerComponent m_player;
        private OverlayView m_view;
        private InventoryView m_inventoryView;
        private CrateView m_crateView;

        public void Initialize(
            PlayerComponent player, 
            OverlayView view, 
            InventoryView inventoryView,
            CrateView crateView
            )
        {
            m_player = player;
            m_view = view;
            m_inventoryView = inventoryView;
            m_crateView = crateView;

            ConnectModel();
            ConnectView();
            ConnectInventoryView();
            ConnectCrateView();
        }

        

        // General Logic ----------------------------------------------
        private void Update()
        {
            if (!m_view.IsActive) return;

            m_view.SetActiveInteractionButton(m_player.ClosestInteractable != null);

            if (m_player.ClosestInteractable != null && m_player.ClosestInteractable is DestroyableComponent dest)
                m_view.SetInteractionProgress(dest.Progress);
            else
                m_view.SetInteractionProgress(0);
        }
        // Model connection ------------------------------------------
        private void ConnectModel()
        {

        }
        // View connection -------------------------------------------
        private void ConnectView()
        {
            m_view.InteractionStarted += OnInteractionStarted;
            m_view.InventoryOpened += OnInventoryOpen;
        }
        private void OnInventoryOpen()
        {
            m_view.SetActive(false);
        }
        private void OnInteractionStarted()
        {
        }
        // Inventory View connection -----------------------------------
        private void ConnectInventoryView()
        {
            m_inventoryView.ExitClicked += OnInventoryClosed;
        }
        private void OnInventoryClosed()
        {
            m_view.SetActive(true);
        }
        // Crate View connection ---------------------------------------
        private void ConnectCrateView()
        {
            m_crateView.Closed += OnCrateClosed;
        }
        private void OnCrateClosed()
        {
            m_view.SetActive(true);
        }
    }
}
