using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public abstract class Entity : Actor, ITargetable, ILootable
    {
        public float _BaseMaxHealth = 100.0f;
        public float _BaseHealth = 100.0f;
        public float _BaseSharpArmor = 0.0f;
        public float _BaseBluntArmor = 0.0f;
        public float _BaseBallisticArmor = 0.0f;
        public float _BaseExplosiveArmor = 0.0f;
        public float _BaseEnergyArmor = 0.0f;
        public bool _Immortal = false;
        public bool _Lootable = true;

        public ControllerData _Controller;
        public readonly List<AbilityData> _Abilities = new List<AbilityData>();
        public readonly InventoryData _Inventory = new InventoryData();

        public float _MaxHealth { get; private set; }
        public float _Health { get; private set; }

        protected virtual void Start()
        {
            SetMaxHealth(CalculateMaxHealth());
            SetHealth(CalculateHealth());
            CheckAlive();
        }

        protected virtual void Update()
        {
            
        }

        #region Main
        public void SetMaxHealth(float maxHealth)
        {
            _MaxHealth = _MaxHealth;
            if (_Health > _MaxHealth) SetHealth(_MaxHealth);
        }

        public void SetHealth(float health)
        {
            _Health = health;
            if (_Health > _MaxHealth) _Health = _MaxHealth;
            CheckAlive();
        }

        public void SetImmortal(bool immortal)
        {
            _Immortal = immortal;
        }

        public void CheckAlive()
        {
            if (!IsAlive()) Die();
        }

        public bool IsAlive()
        {
            return _Health > 0;
        }

        public void Die()
        {
            _Health = 0;
            OnDeath();
        }

        public void Revive(int health)
        {
            SetHealth(health);
            OnRevive();
        }

        public virtual bool IsTargetable()
        {
            return true;
        }

        public void Damage(DamageData damageData)
        {
            float damage = damageData._Damage;
            if (_Immortal) damage = 0.0f;

            //TODO: add damage effects (IE: damage numbers/colors based on damage type), let attack method do damage calculations
            switch (damageData._DamageType)
            {
                case DamageData.DamageType.Sharp:
                    break;
                case DamageData.DamageType.Blunt:
                    break;
                case DamageData.DamageType.Ballistic:
                    break;
                case DamageData.DamageType.Explosive:
                    break;
                case DamageData.DamageType.Energy:
                    break;
            }

            SetHealth(damage);
        }

        public virtual bool IsLootable()
        {
            if (!_Lootable || IsAlive()) return false;
            return true;
        }

        public virtual void Loot()
        {

        }
        #endregion

        #region Calculate Totals
        public virtual float CalculateMaxHealth()
        {
            return _BaseMaxHealth;
        }

        public virtual float CalculateHealth()
        {
            return _BaseHealth;
        }

        public virtual float CalculateSharpArmor()
        {
            return _BaseSharpArmor;
        }

        public virtual float CalculateBluntArmor()
        {
            return _BaseBluntArmor;
        }

        public virtual float CalculateBallisticArmor()
        {
            return _BaseBallisticArmor;
        }

        public virtual float CalculateExplosiveArmor()
        {
            return _BaseExplosiveArmor;
        }

        public virtual float CalculateEnergyArmor()
        {
            return _BaseEnergyArmor;
        }
        #endregion

        #region Callbacks
        public abstract void OnRevive();
        public abstract void OnDeath();
        #endregion
    }
}
