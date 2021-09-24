using UnityEngine;

namespace Balloondle.Gameplay.Physics2D
{
    public class Rope2DBreakerWhenTraversed : MonoBehaviour
    {
        private float _traversedRadiusDetection = 0.00025f;
        private Collider2D _sourceCollider2D;

        private void OnEnable()
        {
            _sourceCollider2D = GetComponent<Collider2D>();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (_sourceCollider2D == null)
            {
                return;
            }

            bool breakJoint = false;

            Vector2 sourcePosition = _sourceCollider2D.attachedRigidbody.position;
            Vector2 firstCheckPoint = Vector2.zero;
            Vector2 secondCheckPoint = Vector2.zero;
            
            foreach (var contact in other.contacts)
            {
                if (_sourceCollider2D == null || contact.collider == null)
                {
                    return;
                }

                if (_sourceCollider2D.attachedRigidbody == null || contact.collider.attachedRigidbody == null)
                {
                    return;
                }

                Vector2 direction = _sourceCollider2D.attachedRigidbody.position - 
                                    contact.collider.attachedRigidbody.position;
                direction.Normalize();

                firstCheckPoint.Set(sourcePosition.x + _traversedRadiusDetection * direction.x,
                    sourcePosition.y + _traversedRadiusDetection * direction.y);
                secondCheckPoint.Set(sourcePosition.x - _traversedRadiusDetection * direction.x,
                    sourcePosition.y - _traversedRadiusDetection * direction.y);
                
                if (contact.collider.OverlapPoint(firstCheckPoint) && contact.collider.OverlapPoint(secondCheckPoint))
                {
                    breakJoint = true;
                    break;
                }
            }

            if (breakJoint)
            {
                GetComponent<HingeJoint2D>().breakForce = 0f;
            }
        }
    }
}