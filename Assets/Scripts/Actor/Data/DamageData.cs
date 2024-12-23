using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public class DamageData
    {
        public enum DamageType
        {
            Sharp,
            Blunt,
            Ballistic,
            Explosive,
            Energy,
            Other
        }

        public float _Damage { get; private set; }
        public DamageType _DamageType { get; private set; }

        public DamageData(float damage, DamageType damageType)
        {
            _Damage = damage;
            _DamageType = damageType;
        }
    }
}
