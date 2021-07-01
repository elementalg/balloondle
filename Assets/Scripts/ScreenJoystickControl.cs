using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;

/// <summary>
/// A stick control displayed on screen and moved around by touch or other pointer
/// input.
/// </summary>
[AddComponentMenu("Input/On-Screen Stick")]
public class ScreenJoystickControl : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    /// <summary>
    /// Maximum amount of displacement that can be applied to the joystick's dot.
    /// </summary>
    [FormerlySerializedAs("movementRange")]
    [SerializeField]
    private float m_MovementRange = 50;

    /// <summary>
    /// Control to which the input, obtained from the player's touches, is transferred to.
    /// </summary>
    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;
    
    /// <summary>
    /// Neutral position, where the joystick's dot is in the center.
    /// </summary>
    private Vector3 _startPos;
    
    /// <summary>
    /// Position where the player has started touching the screen.
    /// </summary>
    private Vector2 _pointerDownPos;
    
    /// <summary>
    /// Maximum amount of displacement that can be applied to the joystick's dot.
    /// </summary>
    public float MovementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }

    /// <summary>
    /// Neutral position, where the joystick's dot is in the center.
    /// </summary>
    public Vector3 StartPos
    {
        get => _startPos;
        set => _startPos = value;
    }

    /// <summary>
    /// Position where the player has started touching the screen.
    /// </summary>
    public Vector2 PointerDownPos
    {
        get => _pointerDownPos;
        set => _pointerDownPos = value;
    }

    /// <summary>
    /// Control to which the input, obtained from the player's touches, is transferred to.
    /// </summary>
    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
    
    private void Start()
    {
        _startPos = ((RectTransform)transform).anchoredPosition;
    }
    
    /// <summary>
    /// Establish the origin position which will be used for calculating the displacement of the joystick.
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.ArgumentNullException">if <paramref name="eventData"/> is null.</exception>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponentInParent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out _pointerDownPos);
    }

    /// <summary>
    /// Calculate the amount of displacement produced through the dragging of the dot.
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.ArgumentNullException">if <paramref name="eventData"/> is null.</exception>
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponentInParent<RectTransform>(), 
            eventData.position,
            eventData.pressEventCamera,
            out var position);
            
        var delta = position - _pointerDownPos;

        delta = Vector2.ClampMagnitude(delta, MovementRange);
        ((RectTransform)transform).anchoredPosition = _startPos + (Vector3)delta;

        var newPos = new Vector2(delta.x / MovementRange, delta.y / MovementRange);
        SendValueToControl(newPos);
    }

    /// <summary>
    /// Make the joystick go back to its center position.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        ((RectTransform)transform).anchoredPosition = _startPos;
        SendValueToControl(Vector2.zero);
    }
}