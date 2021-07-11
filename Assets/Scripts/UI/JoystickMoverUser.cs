using System;
using UnityEngine;

namespace Balloondle.UI
{
    public class JoystickMoverUser : MonoBehaviour
    {
        [SerializeField, Tooltip("TouchListener used for handling the touches.")]
        private TouchListener m_TouchListener;

        [SerializeField, Tooltip("Camera used for the game.")] 
        private Camera m_Camera;

        private JoystickRange _joystickRange;
        
        
        private SingleTouchListener _singleTouchListener;
        private RectTransform _rootRectTransform;
        private Rect _rect;

        private void Start()
        {
            if (transform.GetChild(0)?.GetComponent<JoystickRange>() is null)
            {
                throw new InvalidOperationException("JoystickMover requires a JoystickRange child at index 0.");
            }

            _joystickRange = transform.GetChild(0).GetComponent<JoystickRange>();
            
            _singleTouchListener = new SingleTouchListener();

            if (transform.parent.GetComponent<RectTransform>() is null)
            {
                throw new InvalidOperationException("JoystickMover requires parent to have a RectTransform.");
            }
            
            _rootRectTransform = transform.parent.GetComponent<RectTransform>();
            
            if (GetComponent<RectTransform>()?.rect is null)
            {
                throw new InvalidOperationException("JoystickMover requires GameObject to have a RectTransform.");
            }
            
            _rect = GetComponent<RectTransform>().rect;

            m_TouchListener.OnTouchBegan += OnTouchBegan;
            m_TouchListener.OnTouchEnded += OnTouchEnded;
        }

        public void OnTouchBegan(Touch touch)
        {
            if (_singleTouchListener.CurrentlyListening)
            {
                return;
            }

            if (!IsTouchPositionWithinBounds(touch))
            {
                return;
            }
            
            _singleTouchListener.OnTouchBegan(touch);

            if (_joystickRange.IsScreenPointWithinBounds(touch.startScreenPosition, m_Camera))
            {
                _joystickRange.OnTouchBeganWithinBounds(touch, m_Camera);
            }
            else
            {
                // TODO: Displace JoystickRange.
            }
            
            // TODO: Custom logic -> Check if touch is made on the joystick or within the joystick's range. Otherwise,
            // proceed to move the joystick.
        }

        private void UpdateJoystickRangePosition()
        {
            
        }

        private bool IsTouchPositionWithinBounds(Touch touch)
        {
            return CustomRectUtils.Instance.IsScreenPointWithinRect(
                touch.startScreenPosition,
                m_Camera,
                _rootRectTransform,
                _rect);
        }

        public void OnTouchDrag(Touch touch)
        {
            if (!_singleTouchListener.IsTouchBeingListened(touch))
            {
                return;
            }
            
            // TODO: Custom logic.
        }
        
        public void OnTouchEnded(Touch touch)
        {
            if (!_singleTouchListener.IsTouchBeingListened(touch))
            {
                return;
            }
            
            // TODO: Custom logic.
            
            _singleTouchListener.OnTouchEnded(touch);
        }
    }
}
