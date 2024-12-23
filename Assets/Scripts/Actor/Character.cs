using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public class Character : Entity
    {
        //Limbs
        //Stats
        //Perks

        void Start()
        {

        }

        void Update()
        {

        }

        #region Calculate Totals
        public override float CalculateMaxHealth()
        {
            return _BaseMaxHealth;
        }

        public override float CalculateHealth()
        {
            return _BaseHealth;
        }

        public override float CalculateSharpArmor()
        {
            return _BaseSharpArmor;
        }

        public override float CalculateBluntArmor()
        {
            return _BaseBluntArmor;
        }

        public override float CalculateBallisticArmor()
        {
            return _BaseBallisticArmor;
        }

        public override float CalculateExplosiveArmor()
        {
            return _BaseExplosiveArmor;
        }

        public override float CalculateEnergyArmor()
        {
            return _BaseEnergyArmor;
        }
        #endregion

        #region Callbacks
        public override void OnDeath()
        {

        }

        public override void OnRevive()
        {

        }
        #endregion
    }
}
