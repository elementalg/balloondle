using System;
using UnityEngine;

namespace Balloondle.Gameplay.World
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField, Tooltip("Anchor where the Player is attached to.")]
        private Vector3 m_Anchor;
        
        [SerializeField, Tooltip("Maximum damage which this weapon can apply.")] 
        private float m_DamageCapacity = 1f;
        
        private WorldEntity _weaponEntity;
        private BoxCollider2D _boxCollider2D;
        private EdgeCollider2D _edgeCollider2D;
        
        public Vector3 Anchor => m_Anchor;

        private void OnEnable()
        {
            if (GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("Weapon requires a WorldEntity component.");
            }

            _weaponEntity = GetComponent<WorldEntity>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _edgeCollider2D = GetComponent<EdgeCollider2D>();
        }

        public void OnAttachToPlayer()
        {
            _boxCollider2D.enabled = false;
        }

        public void OnDetachFromPlayer()
        {
            _boxCollider2D.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_edgeCollider2D.IsTouching(other))
            {
                WorldEntity otherEntity = other.gameObject.GetComponent<WorldEntity>();

                if (otherEntity != null)
                {
                    otherEntity.Damage(m_DamageCapacity);
                }
            }
        }
    }
}