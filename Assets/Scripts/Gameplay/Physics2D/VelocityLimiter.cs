using System;
using UnityEngine;

namespace Balloondle.Gameplay.Physics2D
{
    /// <summary>
    /// Limits a <see cref="Rigidbody2D"/>'s linear and angular velocity as specified through the serialized fields.
    ///
    /// Caution: The linear drag, and the angular drag are restored to the starting values.
    /// Thus, if you proceed to modify their value, they will get reset
    /// to the values detected when this component has been enabled.
    /// </summary>
    public class VelocityLimiter : MonoBehaviour
    {
        [SerializeField] private float m_MaximumSquaredVelocity = 144f;
        [SerializeField] private float m_SquaredVelocityDifferenceToDragCoefficient = 0.1f;
        
        [SerializeField] private float m_MaximumAngularVelocity = 160f;
        [SerializeField] private float m_AngularVelocityDifferenceToDragCoefficient = 0.005f;
        
        private Rigidbody2D _rigidbody2D;
        private float _dragOnStart;
        private float _angularDragOnStart;
        
        private void OnEnable()
        {
            if (GetComponent<Rigidbody2D>() == null)
            {
                throw new InvalidOperationException(
                    "VelocityLimiter requires the GameObject to contain a RigidBody2D component.");
            }

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _dragOnStart = _rigidbody2D.drag;
            _angularDragOnStart = _rigidbody2D.angularDrag;
        }

        private void FixedUpdate()
        {
            _rigidbody2D.drag = _dragOnStart;
            
            if (_rigidbody2D.velocity.sqrMagnitude > m_MaximumSquaredVelocity)
            {
                _rigidbody2D.drag += (_rigidbody2D.velocity.sqrMagnitude - m_MaximumSquaredVelocity)
                    * m_SquaredVelocityDifferenceToDragCoefficient;
            }

            _rigidbody2D.angularDrag = _angularDragOnStart;
            
            if (Mathf.Abs(_rigidbody2D.angularVelocity) > m_MaximumAngularVelocity)
            {
                _rigidbody2D.angularDrag += (Mathf.Abs(_rigidbody2D.angularVelocity) - m_MaximumAngularVelocity) *
                                            m_AngularVelocityDifferenceToDragCoefficient;
            }
        }
    }
}
