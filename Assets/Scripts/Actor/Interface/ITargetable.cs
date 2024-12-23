using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public interface ITargetable
    {
        bool IsTargetable();
        void Damage(DamageData damageData);
    }
}
