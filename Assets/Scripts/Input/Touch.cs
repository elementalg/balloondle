using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Balloondle.Input
{
    public class Touch
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
    }
}