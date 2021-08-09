using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Balloondle.Input
{
    /// <summary>
    /// Action provider for the main phases of the possible touches.
    /// </summary>
    public class TouchListener : MonoBehaviour
    {
        public Action<IPointerPress> OnTouchUpdate;

        private Touch _touchContainer;
        
        /// <summary>
        /// Enable enhanced touch support at start.
        /// </summary>
        private void Start()
        {
            EnhancedTouchSupport.Enable();
            _touchContainer = new Touch();
        }

        /// <summary>
        /// Check all active touches, and call actions based on each touch's phase.
        /// </summary>
        private void Update()
        {
            foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
            {
                _touchContainer.CloneEnhancedTouch(touch);
                
                OnTouchUpdate?.Invoke(_touchContainer);
            }
        }

        /// <summary>
        /// Disable enhanced touch support when disabled.
        /// </summary>
        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }
    }
}