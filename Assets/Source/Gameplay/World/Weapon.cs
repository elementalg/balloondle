using System;
using Balloondle.Gameplay.World.Effects.Implementations;
using UnityEngine;

namespace Balloondle.Gameplay.World
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField, Tooltip("Anchor where the Player is attached to.")]
        private Vector3 m_Anchor;
        
        [SerializeField, Tooltip("Maximum damage which this weapon can apply.")] 
        private float m_DamageCapacity = 1f;

        [SerializeField, Tooltip("Minimum squared velocity for damage to be applied.")]
        private float m_MinSquaredVelocityForDamage = 10f;

        [SerializeField, Tooltip("Maximum squared velocity for maximum damage.")]
        private float m_MaxSquaredVelocityForMaxDamage = 100f;
        
        private WorldEntity _weaponEntity;
        private BoxCollider2D _boxCollider2D;
        private EdgeCollider2D _edgeCollider2D;
        private WeaponEffects _weaponEffects;
        
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
            
            if (GetComponent<WeaponEffects>() == null)
            {
                throw new InvalidOperationException("Weapon requires a WeaponEffects component.");
            }
            
            _weaponEffects = GetComponent<WeaponEffects>();
        }

        public void OnAttachToPlayer()
        {
            _boxCollider2D.enabled = false;
        }

        public void OnDetachFromPlayer()
        {
            _boxCollider2D.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            WorldEntity otherEntity = other.gameObject.GetComponent<WorldEntity>();
            
            if (otherEntity == null)
            {
                return;
            }

            if (otherEntity.m_Indestructible)
            {
                return;
            }
            
            if (other.otherCollider != _edgeCollider2D || other.gameObject == gameObject)
            {
                return;
            }

            if (other.relativeVelocity.sqrMagnitude < m_MinSquaredVelocityForDamage)
            {
                return;
            }
            
            float hitIntensity = Mathf.Min(1f,
                other.relativeVelocity.sqrMagnitude / m_MaxSquaredVelocityForMaxDamage);

            float damageApplied = m_DamageCapacity * hitIntensity;

            if (damageApplied < otherEntity.Health)
            {
                _weaponEffects.PlayHitEffect(hitIntensity);
            }
            
            otherEntity.Damage(damageApplied);
        }
    }
}