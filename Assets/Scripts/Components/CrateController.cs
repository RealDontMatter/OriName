using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Views;

namespace Components
{
    public class CrateController : MonoBehaviour
    {
        private Inventory m_playerInventory, m_crateInventory;

        private CrateView m_view;
        private OverlayView m_overlayView;

        public void Initialize(
            Inventory playerInventory, 
            
            CrateView view,
            OverlayView overlayView
            )
        {
            m_playerInventory = playerInventory;
            m_crateInventory = null;
            m_view = view;
            m_overlayView = overlayView;

            ConnectPlayerModel();

            ConnectView();
            ConnectOverlayView();
        }
        // Player Model -----------------------------------------------
        private void ConnectPlayerModel()
        {
            m_playerInventory.ItemsChanged += OnPlayerInventoryChanged;
        }

        private void OnPlayerInventoryChanged()
        {
            // Update View
        }

        // Model ------------------------------------------------------
        public void SetCrateModel(Inventory crateInventory)
        {
            m_crateInventory = crateInventory;
            OnCrateInventoryChanged();
            if (m_crateInventory == null) return;

            m_crateInventory.ItemsChanged += OnCrateInventoryChanged;

        }
        public void RemoveCrateModel()
        {
            m_crateInventory.ItemsChanged -= OnCrateInventoryChanged;
            m_crateInventory = null;
        }

        private void OnCrateInventoryChanged()
        {
        }

        // View -------------------------------------------------------
        private void ConnectView()
        {
            m_view.Closed += OnViewClosed;
        }
        private void OnViewClosed()
        {
            m_view.SetActive(false);
        }
        // Overlay View -----------------------------------------------
        private void ConnectOverlayView()
        {
            m_overlayView.CrateOpened += OnCrateOpened;
        }
        private void OnCrateOpened()
        {
            m_view.SetActive();
        }
    }
}
