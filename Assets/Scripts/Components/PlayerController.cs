using Assets.Scripts.SO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using Models.Interfaces;

namespace Components
{
    public class PlayerController : MonoBehaviour
    {
        public IInteractable ClosestInteractable => m_closestInteractable;
        public Models.Inventory Inventory => m_inventory;


        [SerializeField]
        private PlayerSettingsSO m_playerSettings;
        private Models.Inventory m_inventory;
        private List<IInteractable> m_interactables;
        private IInteractable m_closestInteractable;
        private bool m_isInitialized;

        private InputAction m_moveAction, m_useAction;
        private Rigidbody m_rigidbody;


        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            m_moveAction = InputSystem.actions.FindAction("Move");
            m_useAction = InputSystem.actions.FindAction("Use");

            m_interactables = new List<IInteractable>();
        }

        public void Initialize(Models.Inventory inventory)
        {
            if (m_isInitialized) return;
            
            m_inventory = inventory;

            m_isInitialized = true;
        }
        public void RegisterInteractable(IInteractable interactable)
        {
            interactable.Destroying += () => 
            {
                m_interactables.Remove(interactable);
                SelectClosestInteractable();
            };
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
            IInteractable interactable = null;
            foreach (var item in m_interactables)
            {
                if( interactable == null || 
                    Vector3.Distance(transform.position, item.Position) < Vector3.Distance(transform.position, interactable.Position)
                    )
                    interactable = item;
            }
            m_closestInteractable = interactable;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var component))
            {
                m_interactables.Add(component);
                SelectClosestInteractable();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var component))
            {
                m_interactables.Remove(component);
                SelectClosestInteractable();
            }
        }
    }
}