using System;
using UnityEngine;

namespace Balloondle.Gameplay.World
{
    /// <summary>
    /// Detects collisions, awaiting for a collision with a weapon which is currently available.
    /// </summary>
    public class WeaponGrabber : MonoBehaviour
    {
        private Player _player;
        private CircleCollider2D _weaponGrabber;

        private void OnEnable()
        {
            _player = GetComponent<Player>();
            _weaponGrabber = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_player.HasWeapon())
            {
                if (_weaponGrabber.IsTouching(other) && other is BoxCollider2D)
                {
                    Weapon weapon = other.gameObject.GetComponent<Weapon>();

                    if (weapon != null)
                    {
                        _player.GiveWeapon(weapon.GetComponent<WorldEntity>());
                    }
                }
            }
        }
    }
}