using Models;
using System;
using UnityEngine;
using Views;

namespace Components
{
    class InventoryInterfaceController : MonoBehaviour
    {
        private InventoryView m_view;
        private OverlayView m_overlayView;
        private Inventory m_inventory;


        public void Initialize(Inventory inventory, InventoryView view, OverlayView overlayView)
        {
            m_view = view;
            m_inventory = inventory;
            m_overlayView = overlayView;

            ConnectModel();
            ConnectView();

            ConnectOverlayView();
        }



        //----------------------------------------------------------
        private int m_selectedSlot = -1;
        private int SelectedSlot
        {
            get => m_selectedSlot;
            set
            {
                if (value >= m_inventory.Size || value < 0 || m_inventory.m_items[value] == null )
                    m_selectedSlot = -1;
                else
                    m_selectedSlot = value;

                if(m_selectedSlot == -1) m_view.DeselectAllItems();
                else m_view.SelectItem(m_selectedSlot);

                m_view.DeleteButtonSetActive(m_selectedSlot != -1);
                m_view.SplitButtonSetActive(CanSplitItem());

            }
        }
        //---------------------------------------------------------------
        private void ConnectModel()
        {
            m_inventory.ItemsChanged += () => { 
                m_view.UpdateItems(m_inventory.m_items);
                SelectedSlot = SelectedSlot;
            };
            foreach(var item in m_inventory.m_items)
            {
                if(item != null)
                    item.Changed += () => {
                        m_view.UpdateItems(m_inventory.m_items);
                        SelectedSlot = SelectedSlot;
                    };
            }
        }
        private bool CanSplitItem()
        {
            if (SelectedSlot == -1) return false;
            var selectedItem = m_inventory.m_items[m_selectedSlot];
            if(selectedItem == null) return false;
            if(selectedItem.Count <= 1) return false;
            if(!m_inventory.HasEmptySlots) return false;
            return true;
        } 

        //-------------------------------------------------------------
        public void SetActive(bool active = true) => m_view.SetActive(active);
        private void ConnectView()
        {
            m_view.ExitClicked += OnExitClick;
            m_view.SlotClicked += OnSlotClicked;
            m_view.SplitClicked += OnSplitClicked;
            m_view.DeleteClicked += OnDeleteClicked;
            m_view.ItemDragged += OnItemDragged;

            // First update
            m_view.UpdateItems(m_inventory.Items);
            SelectedSlot = -1;
        }
        private void OnItemDragged(int from, int to)
        {
            m_inventory.SwapItems(from, to);
        }
        private void OnDeleteClicked() => m_inventory.RemoveItem(m_selectedSlot);
        private void OnSplitClicked() => m_inventory.SplitItem(m_selectedSlot);
        private void OnSlotClicked(int index) => SelectedSlot = index;
        private void OnExitClick()
        {
            m_view.SetActive(false);
        }
        //-------------------------
        private void ConnectOverlayView()
        {
            m_overlayView.InventoryOpened += OnInventoryOpen;
        }
        private void OnInventoryOpen()
        {
            m_view.SetActive(true);
        }
        //------------------------------------------------------------------
    }
}
