using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField, Tooltip("Anchor where the Player is attached to.")]
        private Vector3 m_Anchor;
        
        [SerializeField, Tooltip("Maximum damage which this weapon can apply.")] 
        private float m_DamageCapacity = 1f;
        
        private WorldEntity _weaponEntity;

        public Vector3 Anchor => m_Anchor;

        private void OnEnable()
        {
            if (GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("Weapon requires a WorldEntity component.");
            }

            _weaponEntity = GetComponent<WorldEntity>();
        }
    }
}