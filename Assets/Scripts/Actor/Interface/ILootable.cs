using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public interface ILootable
    {
        bool IsLootable();
        void Loot();
    }
}
