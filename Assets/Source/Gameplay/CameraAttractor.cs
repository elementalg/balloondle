using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    /// <summary>
    /// Focus the main camera towards the GameObject containing this component.
    /// </summary>
    public class CameraAttractor : MonoBehaviour
    {
        [SerializeField] 
        private bool m_EnableEasing;

        [SerializeField] 
        private float m_EasingDuration = 1f;

        private Transform _attractedCameraTransform;
        
        private void OnEnable()
        {
            if (Camera.main == null)
            {
                throw new InvalidOperationException("CameraAttractor requires a main camera to be accessible.");
            }
            
            _attractedCameraTransform = Camera.main.transform;

            if (GetComponent<CameraAttractorEasing>() == null)
            {
                if (m_EnableEasing)
                {
                    CameraAttractorEasing attractorEasing = gameObject.AddComponent<CameraAttractorEasing>();
                    attractorEasing.EasingDurationInSeconds = m_EasingDuration;
                    
                    // Disable, and wait for enable from CameraAttractorEasing when easing has been completed.
                    enabled = false;
                } 
            }
            else
            {
                Destroy(GetComponent<CameraAttractorEasing>());
            }
        }

        void Update()
        {
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = new Vector3(currentPosition.x,
                currentPosition.y,
                _attractedCameraTransform.position.z);

            _attractedCameraTransform.position = newPosition;
        }
    }
}
