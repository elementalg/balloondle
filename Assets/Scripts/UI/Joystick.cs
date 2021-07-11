using System;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Balloondle.UI
{
    public class Joystick : MonoBehaviour
    {
        private enum State
        {
            Awaiting,
            Touched,
        }
        
        private RectTransform _parentRectTransform;
        private Rect _rect;

        private Vector2 _startPosition;

        private float _movementRange;

        private Vector2 _touchBeganPosition;
        private Vector2 _latestTouchPosition;

        public void Start()
        {
            if (transform.parent.GetComponent<RectTransform>() == null)
            {
                throw new InvalidOperationException("Joystick requires parent to have a RectTransform.");
            }

            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
            
            // Movement range is the radius of the JoystickRange's circle, thus width or height must be the same here.
            _movementRange = _parentRectTransform.rect.width / 2f;

            if (GetComponent<RectTransform>()?.rect == null)
            {
                throw new InvalidOperationException("Joystick requires GameObject to have a RectTransform.");
            }
            
            _rect = GetComponent<RectTransform>().rect;

            _startPosition = ((RectTransform) transform).anchoredPosition;
            _touchBeganPosition = new Vector2(_startPosition.x, _startPosition.y);
        }
        
        public bool IsScreenPointWithinBounds(Vector2 screenPoint, Camera gameCamera)
        {
            return CustomRectUtils.Instance.IsScreenPointWithinRect(
                screenPoint,
                gameCamera,
                _parentRectTransform,
                _rect);
        }

        public void OnTouchBegan(Touch touch, Camera gameCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRectTransform,
                touch.startScreenPosition,
                gameCamera,
                out _touchBeganPosition);
        }
        
        public void OnTouchDrag(Touch touch, Camera gameCamera)
        {
            UpdateLatestTouchPosition(touch.screenPosition, gameCamera);

            Vector2 delta = _latestTouchPosition - _touchBeganPosition;
            delta = Vector2.ClampMagnitude(delta, _movementRange);
            ((RectTransform) transform).anchoredPosition = _startPosition + delta;

            Vector2 controlValue = new Vector2(delta.x / _movementRange, delta.y / _movementRange);
        }

        private void UpdateLatestTouchPosition(Vector2 screenPosition, Camera gameCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRectTransform,
                screenPosition,
                gameCamera,
                out _latestTouchPosition);
        }

        public void OnTouchEnded(Touch touch)
        {
            // Restore initial position.
            ((RectTransform) transform).anchoredPosition = _startPosition;
        }
    }
}