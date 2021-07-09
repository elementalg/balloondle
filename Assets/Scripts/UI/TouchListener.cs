using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
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
        
        /// <summary>
        /// Enable enhanced touch support at start.
        /// </summary>
        private void Start()
        {
            EnhancedTouchSupport.Enable();
        }

        /// <summary>
        /// Check all active touches, and call actions based on each touch's phase.
        /// </summary>
        private void Update()
        {
            foreach (Touch touch in Touch.activeTouches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (OnTouchBegan != null)
                        {
                            OnTouchBegan(touch);
                        }

                        break;
                    case TouchPhase.Moved:
                        if (OnTouchDrag != null)
                        {
                            OnTouchDrag(touch);
                        }

                        break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        if (OnTouchEnded != null)
                        {
                            OnTouchEnded(touch);
                        }

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
