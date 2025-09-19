using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class CrateView : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject m_slotPrefab;

        [Header("References")]
        [SerializeField] private GameObject m_holder;
        [SerializeField] private GameObject m_inventoryItemsHolder, m_crateItemsHolder;
        
        [SerializeField] private Button m_exitButton;

        // Events
        public event Action Closed;

        private void Start()
        {
            m_exitButton.onClick.AddListener(() => Closed?.Invoke());
        }

        public void SetActive(bool active = true) => m_holder.SetActive(active);
        public bool IsActive => m_holder.activeSelf;
    }
}
