﻿using System;
using UnityEngine;

namespace Balloondle.Gameplay.World
{
    public class Player : MonoBehaviour
    {
        public Action OnWeaponGiven;
        public Action OnWeaponDropped;

        [SerializeField, Tooltip("Anchor where the weapon is attached to.")] 
        private Vector3 m_Anchor;

        [SerializeField, Tooltip("Distance to be kept with the weapon.")]
        private float m_DistanceFromWeapon = 1f;

        private WorldEntity _playerEntity;
        
        private WorldEntity _currentWeapon;

        private void OnEnable()
        {
            if (GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("Player requires a WorldEntity component.");
            }
            
            _playerEntity = GetComponent<WorldEntity>();
        }

        public void GiveWeapon(WorldEntity weapon)
        {
            if (weapon.GetComponent<Weapon>() == null)
            {
                throw new ArgumentException("Weapon must contain a Weapon component.", nameof(weapon));
            }
            
            if (HasWeapon())
            {
                DropWeapon();
            }

            Weapon weaponDetails = weapon.GetComponent<Weapon>();
            
            if (_playerEntity.TryAttachTo(m_Anchor, weapon, weaponDetails.Anchor, true, m_DistanceFromWeapon))
            {
                _currentWeapon = weapon;
                
                OnWeaponGiven?.Invoke();
            }
            else
            {
                Debug.LogWarning("Failed to attach Weapon to Player.");
            }
        }

        public bool HasWeapon()
        {
            return _currentWeapon != null && _playerEntity.IsAttachedTo(_currentWeapon);
        }

        public void DropWeapon()
        {
            _playerEntity.DetachFrom(_currentWeapon);
            _currentWeapon = null;
            
            OnWeaponDropped?.Invoke();
        }
    }
}