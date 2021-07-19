using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Balloondle.UI.Controllers
{
    public class Joystick : OnScreenControl
    {
        [SerializeField]
        private float m_MovementRange = 150f;

        [SerializeField]
        [InputControl(layout = "Vector2")]
        private string m_ControlPath;

        private RectTransform _parentRectTransform;

        protected override string controlPathInternal 
        { 
            get => m_ControlPath;
            set => m_ControlPath = value; 
        }

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