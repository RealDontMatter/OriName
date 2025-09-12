using System;
using UnityEngine;
using Utility;
using Models.Interfaces;
using System.Linq;
using Models;

namespace Components
{
    public class DestroyableComponent : MonoBehaviour
    {
        [SerializeField] private DestroyableType m_type;

        private Destroyable m_model;
        
        private PlayerComponent m_playerComponent;

        private bool m_isInteracting;
        private float m_clock=0;

        public event Action InteractionStarted;
        public event Action InteractionEnded;
        public event Action Destroying;

        public bool IsInteracting => m_isInteracting;
        public float Progress => m_clock / m_model.DestroyableType.HitDuration;
        public Vector3 Position => transform.position;


        public void Interact()
        {
            m_isInteracting = true;
            m_clock = 0;
            InteractionStarted?.Invoke();
        }
        public bool CanInteract()
        {
            if(m_isInteracting) return false;
            
            return m_model.CanInteract(m_playerComponent.Inventory);
        }

        public void Initialize(GameObject player)
        {
            m_playerComponent = player.GetComponent<PlayerComponent>();
            m_model = m_type.CreateInteractable() as Destroyable;

            m_model.Destroyed += OnModelDestroed;

        }

        private void OnModelDestroed()
        {
            m_playerComponent.Inventory.AddItems(m_model.DestroyableType.ItemsToDrop.Select(i => (i.ItemType, i.Count)));
            Destroying?.Invoke();
            Destroy(gameObject);
        }

        void Update()
        {
            if (!m_isInteracting) return;
            
            m_clock += Time.deltaTime;
            
            if (m_clock >= m_model.DestroyableType.HitDuration)
            {
                m_isInteracting = false;
                m_clock = 0;
                m_model.Hit();

                var toolType = m_model.DestroyableType.ToolType;
                if (toolType != null)
                {
                    var inventory = m_playerComponent.Inventory;
                    var itemToConsume = inventory.GetItem(toolType) as ConsumableItem;
                    itemToConsume.Consume();
                }
                InteractionEnded?.Invoke();
            }
        }
    }
}
