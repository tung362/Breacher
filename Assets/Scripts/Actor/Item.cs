using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public class Item : Actor, IPickable
    {
        public int _Stack = 1;

        public bool IsPickable()
        {
            return true;
        }

        public void Pick(Entity entity)
        {
            //TODO: Add to inventory
            Destroy(gameObject);
        }
    }
}
