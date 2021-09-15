using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    /// <summary>
    /// Basic entity present in the game's world. It defines the basic stats of each destroyable in game entity:
    ///
    /// - Health.
    /// - Armor.
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

        public float Health { get; private set; } = 100f;
        public float DestroyAfterTime { get; set; } = 1f;

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
            
            // If the sum goes to infinite, proceed to limit it to the maximum real float number.
            if (healAmount + Health >= float.MaxValue)
            {
                Health = float.MaxValue;
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
                    Destroy(gameObject, DestroyAfterTime);
                #endif
                return;
            }

            Health -= damageAmount;
        }
    }
}
