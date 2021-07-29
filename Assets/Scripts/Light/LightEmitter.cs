using UnityEngine;

namespace Balloondle.Light
{
    // TODO -> Link the required component.
    /// <summary>
    /// Generates raycasts in a circular manner, and when a body is hit, the hit is transferred for further
    /// handling if the body contains the required component.
    /// </summary>
    public class LightEmitter : MonoBehaviour
    {
        [SerializeField, Tooltip("Amount of raycasts which will be distanced evenly.")]
        private ushort m_AmountOfRaycasts = 180;

        [SerializeField, Tooltip("Maximum distance which the raycast will travel.")]
        private float m_MaximumRaycastDistance = 10f;

        /// <summary>
        /// Value holder for the angle at which a raycast must be emitted.
        /// The angle is calculated in a counter-clockwise manner.
        /// </summary>
        private float _angleIncrementPerRaycast;
        
        /// <summary>
        /// Direction followed by the raycast, relative to the angle's cos (x) and sin (y).
        /// </summary>
        private Vector2 _raycastDirection;
        
        private void Start()
        {
            _angleIncrementPerRaycast = 2 * Mathf.PI / m_AmountOfRaycasts;
            _raycastDirection = new Vector2();
        }

        private void FixedUpdate()
        {
            // Generate raycasts in a circular manner.
            float angleForRaycast = 0f;
            
            for (ushort i = 0; i < m_AmountOfRaycasts; i++)
            {
                _raycastDirection.x = Mathf.Cos(angleForRaycast);
                _raycastDirection.y = Mathf.Sin(angleForRaycast);

                RaycastHit2D hitBody =
                    Physics2D.Raycast(transform.position, _raycastDirection, m_MaximumRaycastDistance);
                
                // TODO -> Handle the hit body
                
                angleForRaycast += _angleIncrementPerRaycast;
            }
        }
    }
}