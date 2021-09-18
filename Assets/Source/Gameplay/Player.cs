using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class Player : MonoBehaviour
    {
        public Action OnWeaponGiven;
        public Action OnWeaponDropped;

        [SerializeField, Tooltip("Anchor where the weapon is attached to.")] 
        private Vector3 m_Anchor; 

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
            
            if (_playerEntity.TryAttachTo(m_Anchor, weapon, weaponDetails.Anchor))
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