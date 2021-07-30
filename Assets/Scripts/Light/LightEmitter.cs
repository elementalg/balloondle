using UnityEngine;

namespace Balloondle.Light
{
    /// <summary>
    /// Generates raycasts in a circular manner, and when a body is hit, the hit is transferred for further
    /// handling if the body contains the <see cref="LightHandler"/> component.
    /// </summary>
    public class LightEmitter : MonoBehaviour
    {
        [SerializeField, Tooltip("Amount of raycasts which will be distanced evenly.")]
        private ushort m_AmountOfRaycasts = 12;

        [SerializeField, Tooltip("Maximum distance which the raycast will travel.")]
        private float m_MaximumRaycastDistance = 10f;

        [SerializeField, Tooltip("Starting and ending angle, in radians," +
                                 " defining the range of the angles used for the creation of raycasts.")]
        private Vector2 m_AngleRange = new Vector2(0f, 2f * Mathf.PI);

        [SerializeField, Tooltip("Size of the capsule.")]
        private Vector2 m_CapsuleSize = new Vector2(0.25f, 1f);

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
            _angleIncrementPerRaycast = (m_AngleRange.y - m_AngleRange.x) / m_AmountOfRaycasts;
            _raycastDirection = new Vector2();
        }

        private void FixedUpdate()
        {
            // Generate raycasts in a circular manner within the specified angle range.
            float angleForRaycast = m_AngleRange.x;
            
            for (ushort i = 0; i < m_AmountOfRaycasts; i++)
            {
                _raycastDirection.x = Mathf.Cos(angleForRaycast);
                _raycastDirection.y = Mathf.Sin(angleForRaycast);

                RaycastHit2D hitBody =
                    Physics2D.CapsuleCast(transform.position, m_CapsuleSize, CapsuleDirection2D.Vertical, 
                        angleForRaycast * 180f / Mathf.PI, _raycastDirection, m_MaximumRaycastDistance);

                if (hitBody.transform != null)
                {
                    LightHandler lightHandler = hitBody.transform.GetComponent<LightHandler>();
                    lightHandler?.OnLightRaycastHit(hitBody);
                }

                // TODO -> Handle the hit body
                
                angleForRaycast += _angleIncrementPerRaycast;
            }
        }
    }
}