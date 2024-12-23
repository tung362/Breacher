using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public interface IInteractable
    {
        bool IsInteractable();
        void Interact();
    }
}
