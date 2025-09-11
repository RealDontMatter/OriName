using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Interactable
    {
        protected InteractableType m_type;
        public virtual InteractableType Type => m_type;
    }
}
