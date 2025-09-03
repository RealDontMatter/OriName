using System;
using UnityEngine;
using Utility;

namespace Components
{
    public class Destroyable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Models.DestroyableData m_destroyableData;

        private Models.Destroyable m_data;
        private GameObject m_player;
        private bool m_isInteracting;
        private float m_clock;

        public event Action InteractionStarted;
        public event Action InteractionEnded;

        public bool IsInteracting => m_isInteracting;
        public float Progress => m_clock / m_data.Data.HitDuration;
        public Vector3 Position => transform.position;


        public void Interact()
        {
            m_isInteracting = true;
            m_clock = 0;

            InteractionStarted?.Invoke();

            if (m_data.HitPoints <= 0)
            {
                m_isInteracting = false;
                CollectReward();
                InteractionEnded?.Invoke();
                return;
            }
        }
        public bool CanInteract() => m_isInteracting == false;

        public void Initialize(GameObject player)
        {
            m_player = player;
            m_data = m_destroyableData.CreateDestroyable();
        }

        void Update()
        {
            if (!m_isInteracting) return;
            
            m_clock += Time.deltaTime;
            
            if (m_clock >= m_data.Data.HitDuration)
            {
                m_clock = 0;
                m_data.HitPoints--;
                m_isInteracting = false;
            }
            if (m_data.HitPoints <= 0) 
            { 
                CollectReward();
                InteractionEnded?.Invoke();
            }
        }

        void CollectReward()
        {
            PlayerController playerCon = m_player.GetComponent<PlayerController>();
            playerCon.Inventory.AddItems(m_data.Data.ItemsToDrop.DeepClone());
            Destroy(gameObject);
        }
    }
}
