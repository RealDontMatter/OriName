using System;
using UnityEngine;
using Utility;
using Models.Interfaces;
using System.Linq;
using Models;

namespace Components
{
    public class Destroyable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Models.DestroyableData m_destroyableData;

        private Models.Destroyable m_data;
        
        private GameObject m_player;
        private PlayerController m_playerComponent;

        private bool m_isInteracting;
        private float m_clock=0;

        public event Action InteractionStarted;
        public event Action InteractionEnded;
        public event Action Destroying;

        public bool IsInteracting => m_isInteracting;
        public float Progress => m_clock / m_data.Data.HitDuration;
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

            PlayerController playerController = m_player.GetComponent<PlayerController>();
            if (m_data.Data.ToolType != null && !playerController.Inventory.HasItem(m_data.Data.ToolType)) return false;

            return true;

        }

        public void Initialize(GameObject player)
        {
            m_player = player;
            m_playerComponent = m_player.GetComponent<PlayerController>();
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

                var toolType = m_data.Data.ToolType;

                Debug.Log("Test");

                if (toolType == null)
                {
                    InteractionEnded?.Invoke();
                    return;
                }

                var inventory = m_playerComponent.Inventory;
                var itemToConsume = inventory.GetItem(toolType);

                if (itemToConsume == null)
                {
                    Debug.LogError($"Destroyable.Update: Expected to find item '{toolType.Name}' in inventory but it was not found.");
                    InteractionEnded?.Invoke();
                    return;
                }

                if (itemToConsume is ConsumableItem consumable)
                {
                    Debug.Log("Test");
                    consumable.Consume();
                    Debug.Log($"Item '{toolType.Name}' has been used to hit the destroyable. Remaining Hitpoints: {consumable.HitPoints}");
                    if (consumable.IsEmpty)
                    {
                        inventory.RemoveItem(inventory.Items.IndexOf(consumable));
                        Debug.Log($"Item '{toolType.Name}' has been consumed and removed from inventory.");
                    }
                }
                else
                {
                    Debug.LogError($"Destroyable.Update: Expected to find item '{toolType.Name}' in inventory but it was not found.");
                }
                InteractionEnded?.Invoke();
            }
            if (m_data.HitPoints <= 0) 
            {
                m_playerComponent.Inventory.AddItems(m_data.Data.ItemsToDrop.DeepClone());
                Destroying?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
