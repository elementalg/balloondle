using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class VelocityLimiter : MonoBehaviour
    {
        [SerializeField] private float m_MaximumSquaredVelocity = 144f;
        [SerializeField] private float m_SquaredVelocityDifferenceToDragCoefficient = 0.1f;
        
        private Rigidbody2D _rigidbody2D;
        private float _dragOnStart;
        
        private void OnEnable()
        {
            if (GetComponent<Rigidbody2D>() == null)
            {
                throw new InvalidOperationException(
                    "VelocityLimiter requires the GameObject to contain a RigidBody2D component.");
            }

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _dragOnStart = _rigidbody2D.drag;
        }

        private void FixedUpdate()
        {
            if (_rigidbody2D.velocity.sqrMagnitude > m_MaximumSquaredVelocity)
            {
                _rigidbody2D.drag = _dragOnStart + (_rigidbody2D.velocity.sqrMagnitude - m_MaximumSquaredVelocity)
                    * m_SquaredVelocityDifferenceToDragCoefficient;
            }
            else
            {
                _rigidbody2D.drag = _dragOnStart;
            }
        }
    }
}
