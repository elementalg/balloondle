using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    /// <summary>
    /// Basic entity present in the game's world. It defines the basic stats of each destroyable in game entity:
    ///
    /// - Health.
    ///
    /// All the default values are set to 0.
    /// </summary>
    public class WorldEntity : MonoBehaviour
    {
        /// <summary>
        /// Called before applying the healing to the WorldEntity's health.
        ///
        /// [PARAMETERS]
        /// - float: amount of healing received.
        /// </summary>
        public Action<float> OnHealthPreReceived;
        
        /// <summary>
        /// Called before applying the damage received to the WorldEntity's health.
        ///
        /// [PARAMETERS]
        /// - float: amount of damage received.
        /// </summary>
        public Action<float> OnDamagePreReceived;
        
        /// <summary>
        /// Called before calling the Destroy() on the WorldEntity's GameObject.
        ///
        /// [PARAMETERS]
        /// - float: amount of damage applied to the WorldEntity, which made its health reach 0.
        /// </summary>
        public Action<float> OnPreDestroy;

        [SerializeField, Tooltip("Time to wait, after the WorldEntity has reached 0, before destroying the object.")]
        private float m_DestroyAfterTime = 1f;

        [SerializeField, Tooltip("Maximum health of the object.")]
        private float m_MaxHealth = Single.MaxValue;

        [SerializeField, Tooltip("Starting health of the object.")]
        private float m_StartingHealth = 100f;
        
        public float Health { get; private set; } = 100f;

        private void OnEnable()
        {
            Health = m_StartingHealth;
        }

        /// <summary>
        /// Only increases the entity's health. When positive infinity is reached, health's value is clamped to the
        /// maximum real <see cref="float"/> number.
        /// </summary>
        /// <param name="healAmount">Amount of health which will be added to the current health.</param>
        public void Heal(float healAmount)
        {
            if (healAmount <= 0f)
            {
                return;
            }

            OnHealthPreReceived?.Invoke(healAmount);
            
            // If the sum exceeds the maximum health, proceed to limit it to the maximum health.
            if (healAmount + Health >= m_MaxHealth)
            {
                Health = m_MaxHealth;
                return;
            }

            Health += healAmount;
        }

        public void Damage(float damageAmount)
        {
            if (damageAmount <= 0f)
            {
                return;
            }
            
            OnDamagePreReceived?.Invoke(damageAmount);
            
            if (Health - damageAmount <= 0f)
            {
                Health = 0f;
                OnPreDestroy?.Invoke(damageAmount);
                
                #if UNITY_EDITOR
                    DestroyImmediate(gameObject);
                #else
                    Destroy(gameObject, m_DestroyAfterTime);
                #endif
                
                return;
            }

            Health -= damageAmount;
        }
    }
}
