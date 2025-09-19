using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class OverlayView : MonoBehaviour
    {
        [SerializeField] private GameObject m_view;
        [SerializeField] private Button m_inventoryButton;

        [SerializeField] private Button m_interactionButton;
        [SerializeField] private Image m_interactionHandImage;
        [SerializeField] private Image m_interactionRadialImage;
        [SerializeField] private Button m_crateButton;

        public Action InteractionStarted, InventoryOpened, CrateOpened;

        public void Initialize()
        {
            m_interactionButton.onClick.AddListener(() => InteractionStarted?.Invoke());
            m_inventoryButton.onClick.AddListener(() => InventoryOpened?.Invoke());
            m_crateButton.onClick.AddListener(() => CrateOpened?.Invoke());
        }

        public void SetActive(bool active = true) => m_view.SetActive(active);
        public bool IsActive => m_view.activeSelf;
        public void SetActiveInteractionButton(bool active = true) => m_interactionHandImage.gameObject.SetActive(active);
        public void SetInteractionProgress(float progress) => m_interactionRadialImage.fillAmount = progress;

    }
}
