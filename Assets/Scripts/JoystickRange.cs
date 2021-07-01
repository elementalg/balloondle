using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Transfers the detected input, from the visual ring defining the maximum movement range,
/// to the joystick's dot "<see cref="ScreenJoystickControl"/>".
/// </summary>
public class JoystickRange : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField, Tooltip("Joystick's movable dot on screen functionality.")]
    private ScreenJoystickControl m_JoystickMovableDotFunctionality;

    public void OnPointerDown(PointerEventData eventData)
    {
        m_JoystickMovableDotFunctionality.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_JoystickMovableDotFunctionality.OnPointerUp(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_JoystickMovableDotFunctionality.OnDrag(eventData);
    }
}