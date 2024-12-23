using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public interface IPickable
    {
        bool IsPickable();
        void Pick(Entity entity);
    }
}
