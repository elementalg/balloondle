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
            HandleInputScreenPoint(screenPoint);
        }

        private void HandleInputScreenPoint(Vector2 screenPoint) 
        {
            // Retrieve screen point's local space positioning, and clamp it to the range of movement.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRectTransform, screenPoint, null, out var localPoint);
            localPoint = Vector2.ClampMagnitude(localPoint, m_MovementRange);
            
            // Move the joystick.
            ((RectTransform)transform).anchoredPosition = localPoint;

            // Transfer the input to the established control.
            SendValueToControl(screenPoint);
        }

        public void InputEnd()
        {
            ResetJoystickState();
        }

        private void ResetJoystickState()
        {
            // Restore the joystick's position to the start.
            ((RectTransform)transform).anchoredPosition = Vector2.zero;

            // Indicate the neutralization of the joystick's position to the control.
            SendValueToControl(Vector2.zero);
        }
    }
}