using UnityEngine;

namespace Balloondle.UI.Controllers
{
    public class Joystick : MonoBehaviour
    {
        [SerializeField]
        private float m_MovementRange = 150f;

        private RectTransform _parentRectTransform;
        
        private void Start() 
        {
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        }

        public void InputUpdate(Vector2 screenPoint)
        {
            UpdateJoystickPosition(screenPoint);
        }

        private void UpdateJoystickPosition(Vector2 screenPoint) 
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRectTransform, screenPoint, null, out var localPoint);
            localPoint = Vector2.ClampMagnitude(localPoint, m_MovementRange);
            
            ((RectTransform)transform).anchoredPosition = localPoint;
        }

        public void InputEnd()
        {
            ((RectTransform)transform).anchoredPosition = Vector2.zero;
        }
    }
}