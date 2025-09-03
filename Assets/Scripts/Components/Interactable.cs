using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Components
{
    public class Interactable : MonoBehaviour
    {
        protected object data;
        protected GameObject player;

        public virtual void Interact() { }
        public virtual bool CanInteract() => true;
        public virtual void Initialize(GameObject player)
        {
            this.player = player;
        }
    }
}
