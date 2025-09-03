using Assets.Scripts.SO;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components
{
    public class PlayerController : MonoBehaviour
    {
        public Components.Interactable ClosestInteractable => m_closestInteractable;
        public Models.Inventory Inventory => m_inventory;


        [SerializeField]
        private PlayerSettingsSO m_playerSettings;
        private Models.Inventory m_inventory;
        private List<Components.Interactable> m_interactables;
        private Components.Interactable m_closestInteractable;
        private bool m_isInitialized;

        private InputAction m_moveAction, m_useAction;
        private Rigidbody m_rigidbody;


        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            m_moveAction = InputSystem.actions.FindAction("Move");
            m_useAction = InputSystem.actions.FindAction("Use");

            m_interactables = new List<Components.Interactable>();
        }

        public void Initialize(Models.Inventory inventory)
        {
            if (m_isInitialized) return;
            
            m_inventory = inventory;

            m_isInitialized = true;
        }

        private void Update()
        {
            var move = m_moveAction.ReadValue<Vector2>();
            Vector3 translate = new(move.x, 0, move.y);
            m_rigidbody.MovePosition(transform.position + transform.rotation * translate * Time.deltaTime * m_playerSettings.MovementSpeed);



            bool use = m_useAction.IsPressed();
            if (use && m_closestInteractable != null && m_closestInteractable.CanInteract())
            {
                m_closestInteractable.Interact();
            }
        }

        private void SelectClosestInteractable()
        {
            Interactable interactable = null;
            foreach (var item in m_interactables)
            {
                if( interactable == null || 
                    Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, interactable.transform.position)
                    )
                    interactable = item;
            }
            m_closestInteractable = interactable;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Interactable>(out var component))
            {
                m_interactables.Add(component);
                SelectClosestInteractable();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Interactable>(out var component))
            {
                m_interactables.Remove(component);
                SelectClosestInteractable();
            }
        }
    }
}