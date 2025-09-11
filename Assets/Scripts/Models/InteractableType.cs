using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Models
{
    public class InteractableType : ScriptableObject
    {
        public virtual Interactable CreateInteractable() => new Interactable();
    }
}
