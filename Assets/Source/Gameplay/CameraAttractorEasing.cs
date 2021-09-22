using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class CameraAttractorEasing : MonoBehaviour
    {
        private Transform _cameraTransform;
        private Vector3 _cameraStartingPosition;
        private Vector3 _objectPosition;

        private float _elapsedTime;

        public float EasingDurationInSeconds { get; set; } = 1f;

        private void Start()
        {
            if (Camera.main == null)
            {
                throw new InvalidOperationException("CameraAttractorEasing requires a main camera to be accessible.");
            }
            
            _cameraTransform = Camera.main.transform;
            _cameraStartingPosition = _cameraTransform.position;

            _objectPosition = transform.position;
            _objectPosition.z = _cameraStartingPosition.z;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            float progress = _elapsedTime / EasingDurationInSeconds;
            
            if (progress > 1f)
            {
                GetComponent<CameraAttractor>().enabled = true;
            }
            else
            {
                _cameraTransform.position = Vector3.Lerp(_cameraStartingPosition, _objectPosition, progress);
            }
        }
    }
}
