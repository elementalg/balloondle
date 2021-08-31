using System;
using UnityEngine;

namespace Balloondle.UI
{
    /// <summary>
    /// Proceeds to change the color of the camera from the start to and end with a linear easing.
    ///
    /// The component destroys itself after finishing the color easing.
    /// </summary>
    public class FadeInOutColorCamera : MonoBehaviour
    {
        [SerializeField, Tooltip("Start Color")]
        private Color m_StartColor;

        [SerializeField, Tooltip("End Color")] 
        private Color m_EndColor;

        [SerializeField, Tooltip("Duration, of the easing, in seconds")]
        private float m_EasingDuration;

        private Camera _camera;
        private float _startTime;
        
        void Start()
        {
            if (GetComponent<Camera>() == null)
            {
                throw new InvalidOperationException("FadeInOutColorCamera requires a Camera component.");
            }
            
            _camera = GetComponent<Camera>();
            _camera.backgroundColor = m_StartColor;

            _startTime = Time.realtimeSinceStartup;
        }

        void Update()
        {
            float progress = (Time.realtimeSinceStartup - _startTime) / m_EasingDuration;

            _camera.backgroundColor = Color.Lerp(m_StartColor, m_EndColor, progress);

            if (progress >= 1f)
            {
                Destroy(this);
            }
        }
    }
}
