using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Balloondle.Gameplay
{
    /// <summary>
    /// Used for moving the balloon relative to the input retrieved. Proceeds to ignore input, whenever
    /// the balloon has been collided by a world's object.
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        public Action OnCharacterMove;
        
        [SerializeField, Tooltip("Maximum velocity")]
        private Vector2 m_MaximumVelocity = new Vector2(5f, 5f);

        [SerializeField, Tooltip("Movement cooldown duration after a collision")]
        private float m_CollisionMovementCooldown;

        private Rigidbody2D _bodyBeingMoved;
        
        private bool _isCollisionCooldownApplied;
        private float _collisionStartTime;

        private EffectApplier _effectApplier;

        private void OnEnable()
        {
            if (GetComponent<Rigidbody2D>() == null)
            {
                throw new InvalidOperationException("MovementController requires a RigidBody2D component.");
            }

            _bodyBeingMoved = GetComponent<Rigidbody2D>();
            
            _effectApplier = GetComponent<EffectApplier>();
        }

        public void OnMove(InputAction.CallbackContext input)
        {
            ApplyMovement(input.ReadValue<Vector2>());
            
            OnCharacterMove?.Invoke();
        }

        private void ApplyMovement(Vector2 inputVelocity)
        {
            if (_isCollisionCooldownApplied)
            {
                if (Time.realtimeSinceStartup - _collisionStartTime > m_CollisionMovementCooldown)
                {
                    _isCollisionCooldownApplied = false;
                }
            }
    
            inputVelocity = inputVelocity * m_MaximumVelocity;

            Vector2 currentVelocity = _bodyBeingMoved.velocity;

            Vector2 movementForce = new Vector2();

            if ((Mathf.Sign(inputVelocity.x) > 0f) != (Mathf.Sign(currentVelocity.x) > 0f))
            {
                movementForce.x = inputVelocity.x + (currentVelocity.x * -1f);
            }
            else
            {
                movementForce.x = inputVelocity.x - currentVelocity.x;
            }

            if ((Mathf.Sign(inputVelocity.y) > 0f) != (Mathf.Sign(currentVelocity.y) > 0f))
            {
                movementForce.y = inputVelocity.y + (currentVelocity.y * -1f);
            }
            else
            {
                movementForce.y = inputVelocity.y - currentVelocity.y;
            }

            // Calculate the force required to obtain the previously calculated velocities.
            movementForce *= _bodyBeingMoved.mass;
        
            if (!_isCollisionCooldownApplied)
            {
                _bodyBeingMoved.AddForce(movementForce, ForceMode2D.Impulse);
            }
        }

        private void OnCollisionEnter2D()
        {
            // Ignore further collisions, if the cooldown is already applied.
            if (_isCollisionCooldownApplied)
            {
                return;
            }
            
            _isCollisionCooldownApplied = true;
            _collisionStartTime = Time.realtimeSinceStartup;

            if (_effectApplier != null)
            {
                _effectApplier.ApplyEffect();
            }
        }
    }
}