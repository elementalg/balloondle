using System;
using System.Collections.Generic;
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
        
        [Tooltip("Time to wait, after the WorldEntity has reached 0, before destroying the object.")]
        public float m_DestroyAfterTime = 1f;

        [Tooltip("Whether or not this object can be destroyed.")]
        public bool m_Indestructible;
        
        [SerializeField, Tooltip("Maximum health of the object.")]
        private float m_MaxHealth = Single.MaxValue;

        [SerializeField, Tooltip("Starting health of the object.")]
        private float m_StartingHealth = 100f;

#nullable enable
        private Dictionary<WorldEntity, WorldEntity>? _attachedTo;
#nullable disable
        
        /// <summary>
        /// Set by <see cref="WorldEntitySpawner"/> when it gets started.
        /// </summary>
        public static WorldEntityAttacher Attacher { get; set; }

        public static bool IsAttachingSupported => Attacher != null;

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

            if (m_Indestructible)
            {
                return;
            }
            
            OnDamagePreReceived?.Invoke(damageAmount);
            
            if (Health - damageAmount <= 0f)
            {
                Health = 0f;
                OnPreDestroy?.Invoke(damageAmount);

                    Destroy(gameObject, m_DestroyAfterTime);
                
                return;
            }

            Health -= damageAmount;
        }

        /// <summary>
        /// Tries to attach this WorldEntity with other one.
        /// </summary>
        /// <param name="anchor"></param>
        /// <param name="otherEntity"></param>
        /// <param name="otherAnchor"></param>
        /// <param name="useCustomDistance"></param>
        /// <param name="customDistance"></param>
        /// <returns>True if attachment succeeded, false otherwise.</returns>
        public bool TryAttachTo(Vector3 anchor, WorldEntity otherEntity, Vector3 otherAnchor,
            bool useCustomDistance = false, float customDistance = 1f)
        {
            if (ReferenceEquals(this, otherEntity))
            {
                return false;
            }
            
#nullable enable
            _attachedTo ??= new Dictionary<WorldEntity, WorldEntity>();
            otherEntity._attachedTo ??= new Dictionary<WorldEntity, WorldEntity>();
#nullable disable
            
            WorldEntity attacher = useCustomDistance ?
                Attacher.Attach(this, anchor, otherEntity, otherAnchor, customDistance) :
                Attacher.Attach(this, anchor, otherEntity, otherAnchor);

            if (_attachedTo.ContainsKey(otherEntity))
            {
                throw new InvalidOperationException("Entity is already attached to OtherEntity.");
            }

            if (otherEntity._attachedTo.ContainsKey(this))
            {
                throw new InvalidOperationException("OtherEntity is already attached to Entity.");
            }
            
            _attachedTo.Add(otherEntity, attacher);
            otherEntity._attachedTo.Add(this, attacher);

            return true;
        }

        /// <summary>
        /// Called in order to proceed to remove the existing attachment between this WorldEntity and other one.
        /// </summary>
        /// <param name="otherEntity"></param>
        public void DetachFrom(WorldEntity otherEntity)
        {
#nullable enable
            _attachedTo ??= new Dictionary<WorldEntity, WorldEntity>();
#nullable disable
            
            if (!_attachedTo.ContainsKey(otherEntity))
            {
                Debug.LogWarning("Entity is not attached to OtherEntity.");
            }

            WorldEntity attacher = _attachedTo[otherEntity];
            
            Attacher.Detach(attacher);
        }

        /// <summary>
        /// To be used <b>EXCLUSIVELY</b> by <see cref="WorldEntityAttacher"/>
        /// in order to synchronize the state of the attachments.
        /// </summary>
        /// <param name="otherEntity"></param>
        public void RemoveAttachmentTo(WorldEntity otherEntity)
        {
#nullable enable
            _attachedTo ??= new Dictionary<WorldEntity, WorldEntity>();
            otherEntity._attachedTo ??= new Dictionary<WorldEntity, WorldEntity>();
#nullable disable
            
            if (!_attachedTo.ContainsKey(otherEntity))
            {
                Debug.LogWarning("Entity is not attached to OtherEntity.");
            }

            if (!otherEntity._attachedTo.ContainsKey(this))
            {
                Debug.LogWarning("OtherEntity is not attached to Entity.");
            }

            _attachedTo.Remove(otherEntity);
            otherEntity._attachedTo.Remove(this);
        }

        public bool IsAttachedTo(WorldEntity otherEntity)
        {
#nullable enable
            _attachedTo ??= new Dictionary<WorldEntity, WorldEntity>();
#nullable disable

            return _attachedTo.ContainsKey(otherEntity);
        }
    }
}