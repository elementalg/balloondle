using System;
using Balloondle.Effects.Implementations;
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
        private WeaponGrabber _weaponGrabber;
        
        private WorldEntity _currentWeapon;

        private void OnEnable()
        {
            if (GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("Player requires a WorldEntity component.");
            }
            
            _playerEntity = GetComponent<WorldEntity>();
            _weaponGrabber = GetComponent<WeaponGrabber>();

            _playerEntity.OnDetachedFrom += OnEntityDetachedFromPlayer;
        }

        public void GiveWeapon(WorldEntity weapon)
        {
            if (weapon.GetComponent<Weapon>() == null)
            {
                throw new ArgumentException("Weapon must contain a Weapon component.", nameof(weapon));
            }
            
            if (HasWeapon())
            {
                Debug.LogWarning("Tried to give weapon to Player, meanwhile having already one.");
                return;
            }

            Weapon weaponDetails = weapon.GetComponent<Weapon>();
            
            if (_playerEntity.TryAttachTo(m_Anchor, weapon, weaponDetails.Anchor, true, m_DistanceFromWeapon))
            {
                _currentWeapon = weapon;
                weaponDetails.OnAttachToPlayer();
                _weaponGrabber.enabled = false;

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

        public void DropWeapon(bool detachWeapon = true)
        {
            if (detachWeapon)
            {
                _playerEntity.DetachFrom(_currentWeapon);
            }

            if (_currentWeapon != null)
            {
                _currentWeapon.GetComponent<Weapon>().OnDetachFromPlayer();
                _weaponGrabber.enabled = true;
                _currentWeapon = null;
            }
           
            
            OnWeaponDropped?.Invoke();
        }

        public WorldEntity GetWeapon()
        {
            return _currentWeapon;
        }

        private void OnEntityDetachedFromPlayer(WorldEntity entity)
        {
            if (entity.GetComponent<Weapon>() != null)
            {
                DropWeapon(false);
            }
        }

        public void CooldownMovement()
        {
            MovementController movementController = GetComponent<MovementController>();
            
            movementController.ApplyCooldown();
            
        }
    }
}