using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Balloondle.UI
{
    /// <summary>
    /// Action provider for the main phases of the possible touches.
    /// </summary>
    public class TouchListener : MonoBehaviour
    {
        public Action<Touch> OnTouchBegan;
        public Action<Touch> OnTouchDrag;
        public Action<Touch> OnTouchEnded;

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
                
                switch (_touchContainer.phase)
                {
                    case TouchPhase.Began:
                        OnTouchBegan?.Invoke(_touchContainer);
                        break;
                    case TouchPhase.Moved:
                        OnTouchDrag?.Invoke(_touchContainer);
                        break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        OnTouchEnded?.Invoke(_touchContainer);
                        break;
                }
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