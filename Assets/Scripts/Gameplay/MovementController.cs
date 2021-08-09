using UnityEngine;
using UnityEngine.InputSystem;

namespace Balloondle.Gameplay
{
    /// <summary>
    /// It is used for moving the balloon relative to the input retrieved. Proceeds to ignore input, whenever
    /// the balloon has been collided by a world's object.
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        [SerializeField, Tooltip("Physics body to which the movement will be applied")]
        private Rigidbody2D m_BodyToBeMoved;

        [SerializeField, Tooltip("Maximum velocity")]
        private Vector2 m_MaximumVelocity = new Vector2(5f, 5f);

        [SerializeField, Tooltip("Movement cooldown duration after a collision")]
        private float m_CollisionMovementCooldown;

        private bool _isCollisionCooldownApplied;
        private float _collisionStartTime;

        public void OnMove(InputAction.CallbackContext input)
        {
            ApplyMovement(input.ReadValue<Vector2>());
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

            Vector2 currentVelocity = m_BodyToBeMoved.velocity;

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
            movementForce *= m_BodyToBeMoved.mass;
        
            if (!_isCollisionCooldownApplied)
            {
                m_BodyToBeMoved.AddForce(movementForce, ForceMode2D.Impulse);
            }
        }

        private void OnCollisionEnter2D()
        {
            _isCollisionCooldownApplied = true;
            _collisionStartTime = Time.realtimeSinceStartup;
        }
    }
}