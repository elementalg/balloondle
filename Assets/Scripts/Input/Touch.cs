using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Balloondle.Input
{
    public class Touch : IPointerPress
    {
        public Vector2 delta { get; set; }
        public Finger finger { get; set; }
        public TouchHistory history { get; set; }
        public TouchPhase phase { get; set; }
        public float pressure { get; set; }
        public Vector2 radius { get; set; }
        public Touchscreen screen { get; set; }
        public Vector2 screenPosition { get; set; }
        public Vector2 startScreenPosition { get; set; }
        public double startTime { get; set; }
        public int tapCount { get; set; }
        public double time { get; set; }
        public int touchId { get; set; }
        public bool valid { get; set; }
        public int pointerId 
        { 
            get => touchId;
            set => touchId = value;
        }
        public PointerPhase pointerPhase
        {
            get 
            {
                return phase switch
                {
                    TouchPhase.Began => PointerPhase.Began,
                    TouchPhase.Moved => PointerPhase.Moved,
                    TouchPhase.Stationary => PointerPhase.Stationary,
                    TouchPhase.Ended => PointerPhase.Ended,
                    TouchPhase.Canceled => PointerPhase.Ended,
                    _ => PointerPhase.None,
                };
            }
            set
            {
                TouchPhase touchPhase = value switch
                {
                    PointerPhase.Began => TouchPhase.Began,
                    PointerPhase.Moved => TouchPhase.Moved,
                    PointerPhase.Stationary => TouchPhase.Stationary,
                    PointerPhase.Ended => TouchPhase.Ended,
                    _ => TouchPhase.None,
                };
                phase = touchPhase;
            }
        }

        public Touch()
        {
            
        }
        
        public Touch(int touchId)
        {
            this.touchId = touchId;
        }
        
        public Touch(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
        {
            CloneEnhancedTouch(touch);
        }

        public void CloneEnhancedTouch(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
        {
            delta = touch.delta;
            finger = touch.finger;
            history = touch.history;
            phase = touch.phase;
            pressure = touch.pressure;
            radius = touch.radius;
            screen = touch.screen;
            screenPosition = touch.screenPosition;
            startScreenPosition = touch.startScreenPosition;
            startTime = touch.startTime;
            tapCount = touch.tapCount;
            time = touch.time;
            touchId = touch.touchId;
            valid = touch.valid;
        }

        public bool HasEnded()
        {
            return phase == TouchPhase.Ended || phase == TouchPhase.Canceled;
        }
    }
}