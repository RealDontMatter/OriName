using System;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    class OverlayInterfaceController : MonoBehaviour
    {
        [SerializeField] Views.OverlayView m_view;

        public Action InventoryOpened, InteractionStarted;


        private PlayerController m_player;

        public void Initialize(PlayerController player)
        {
            m_player = player;
            m_view.InteractionStarted += () => InteractionStarted?.Invoke();
            m_view.InventoryOpened += () => InventoryOpened?.Invoke();
        }

        private void Update()
        {
            if (!m_view.IsActive) return;

            m_view.SetActiveInteractionButton(m_player.ClosestInteractable != null);

            if (m_player.ClosestInteractable != null && m_player.ClosestInteractable is Destroyable dest)
                m_view.SetInteractionProgress(dest.Progress);
            else
                m_view.SetInteractionProgress(0);
        }


        public void SetActive(bool active = true) => m_view.SetActive(active);


    }
}
