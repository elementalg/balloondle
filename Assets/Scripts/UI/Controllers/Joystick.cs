using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Balloondle.UI.Controllers
{
    public class Joystick : OnScreenControl
    {
        private const float BypassInputRepeat = 0.0001f;

        [SerializeField]
        private float m_MovementRange = 150f;

        [SerializeField]
        [InputControl(layout = "Vector2")]
        private string m_ControlPath;

        private RectTransform _parentRectTransform;

        private Vector2 _latestMovement;

        protected override string controlPathInternal 
        { 
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        private void Start() 
        {
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();

            _latestMovement = new Vector2();
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

            Vector2 movement = localPoint / m_MovementRange;

            bool inputRepeated = _latestMovement == movement;

            // If input is repeated, alter the local point, in order to bypass the limiter of repeated input.
            if (inputRepeated)
            {
                movement.x += -1f * Mathf.Sign(movement.x) * (BypassInputRepeat);
                movement.y += -1f * Mathf.Sign(movement.y) * (BypassInputRepeat);
            }

            // Transfer the input to the established control.
            SendValueToControl(movement);

            _latestMovement = movement;
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