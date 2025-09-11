using System;
using UnityEngine;

namespace Models.Interfaces
{
    public interface IInteractable
    {
        Vector3 Position { get; }
        event Action InteractionStarted, InteractionEnded, Destroying;
        void Interact();
        bool CanInteract();
        void Initialize(GameObject player);

    }
}
