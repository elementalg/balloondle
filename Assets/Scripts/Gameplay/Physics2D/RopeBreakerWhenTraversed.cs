using UnityEngine;

namespace Balloondle.Gameplay.Physics2D
{
    public class RopeBreakerWhenTraversed : MonoBehaviour
    {
        private float _traversedRadiusDetection = 0.0005f;
        private Collider2D _sourceCollider2D;

        private void OnEnable()
        {
            _sourceCollider2D = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            bool breakJoint = false;

            Vector2 sourcePosition = _sourceCollider2D.attachedRigidbody.position;
            Vector2 firstCheckPoint = Vector2.zero;
            Vector2 secondCheckPoint = Vector2.zero;
            
            foreach (var contact in other.contacts)
            {
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
