using System;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Balloondle.UI
{
    public class JoystickRange : MonoBehaviour
    {
        private Joystick _joystick;
        
        private RectTransform _parentRectTransform;
        private Rect _rect;

        private void Start()
        {
            if (transform.GetChild(0)?.GetComponent<Joystick>() == null)
            {
                throw new InvalidOperationException("JoystickRange requires a Joystick child at index 0.");
            }
            
            _joystick = transform.GetChild(0).GetComponent<Joystick>();
            
            if (transform.parent.GetComponent<RectTransform>() == null)
            {
                throw new InvalidOperationException("JoystickRange requires parent to have a RectTransform.");
            }
            
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
            
            if (GetComponent<RectTransform>()?.rect == null)
            {
                throw new InvalidOperationException("JoystickRange requires GameObject to have a RectTransform.");
            }
            
            _rect = GetComponent<RectTransform>().rect;
        }

        public bool IsScreenPointWithinBounds(Vector2 screenPoint, Camera gameCamera)
        {
            return CustomRectUtils.Instance.IsScreenPointWithinRect(
                screenPoint,
                gameCamera,
                _parentRectTransform,
                _rect);
        }

        public void OnTouchBeganWithinBounds(Touch touch, Camera gameCamera)
        {
            // TODO: Drag joystick to relative position on joystick's range.
            
            if (_joystick.IsScreenPointWithinBounds(touch.screenPosition, gameCamera))
            {
                _joystick.OnTouchBegan(touch, gameCamera);
            }
            else
            {
                _joystick.OnTouchDrag(touch, gameCamera);
            }
        }
    }
}