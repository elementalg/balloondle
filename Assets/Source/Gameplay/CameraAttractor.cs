using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    /// <summary>
    /// Focus the main camera towards the GameObject containing this component.
    /// </summary>
    public class CameraAttractor : MonoBehaviour
    {
        private Transform _attractedCameraTransform;
        
        private void OnEnable()
        {
            if (Camera.main == null)
            {
                throw new InvalidOperationException("CameraAttractor requires a main camera to be accessible.");
            }
            
            _attractedCameraTransform = Camera.main.transform;
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
