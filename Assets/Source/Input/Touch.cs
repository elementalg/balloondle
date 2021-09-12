using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Balloondle.Input
{
    public class Touch : IPointerPress
    {
        public Vector2 Delta { get; set; }
        public Finger Finger { get; set; }
        public TouchHistory History { get; set; }
        public TouchPhase Phase { get; set; }
        public float Pressure { get; set; }
        public Vector2 Radius { get; set; }
        public Touchscreen Screen { get; set; }
        public Vector2 ScreenPosition { get; set; }
        public Vector2 StartScreenPosition { get; set; }
        public double StartTime { get; set; }
        public int TapCount { get; set; }
        public double Time { get; set; }
        public int TouchId { get; set; }
        public bool Valid { get; set; }
        public int PointerId 
        { 
            get => TouchId;
            set => TouchId = value;
        }
        public PointerPhase PointerPhase
        {
            get 
            {
                return Phase switch
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
                Phase = touchPhase;
            }
        }

        public Touch()
        {
            
        }
        
        public Touch(int touchId)
        {
            this.TouchId = touchId;
        }
        
        public Touch(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
        {
            CloneEnhancedTouch(touch);
        }

        public void CloneEnhancedTouch(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
        {
            Delta = touch.delta;
            Finger = touch.finger;
            History = touch.history;
            Phase = touch.phase;
            Pressure = touch.pressure;
            Radius = touch.radius;
            Screen = touch.screen;
            ScreenPosition = touch.screenPosition;
            StartScreenPosition = touch.startScreenPosition;
            StartTime = touch.startTime;
            TapCount = touch.tapCount;
            Time = touch.time;
            TouchId = touch.touchId;
            Valid = touch.valid;
        }

        public bool HasEnded()
        {
            return Phase == TouchPhase.Ended || Phase == TouchPhase.Canceled;
        }
    }
}