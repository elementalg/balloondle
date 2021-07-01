using UnityEngine;
using UnityEngine.EventSystems;


namespace Balloondle.UI
{
    /// <summary>
    /// Acts as a touch detector that proceeds to move the joystick to the touched position,
    /// in order to make it easier for
    /// players to get as comfortable as possible meanwhile playing a game which makes usage of on screen joysticks.
    /// </summary>
    public class JoystickSurface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private const string MovableDotGameObjectName = "Movable";

        [SerializeField, Tooltip("Percentage margin for the bounds of the possible centers for the joystick.")]
        private Vector2 m_JoystickCenterMargin;

        [SerializeField, Tooltip("Joystick GameObject containing the movable dot, and the range movement.")]
        private GameObject m_Joystick;

        /// <summary>
        /// Joystick's movable dot.
        /// </summary>
        private GameObject _joystickMovable;

        /// <summary>
        /// Script functionality of the joystick's movable dot.
        /// </summary>
        private ScreenJoystickControl _movableJoystickControl;

        /// <summary>
        /// Rectangle defining the possible points that can be used as center points for the joystick.
        /// </summary>
        private Rect _joystickCenterBounds;

        private void Start()
        {
            _joystickMovable = m_Joystick.transform.Find(MovableDotGameObjectName).gameObject;
            _movableJoystickControl = _joystickMovable.GetComponent<ScreenJoystickControl>();

            SetJoystickCenterBounds();
        }

        /// <summary>
        /// Calculates the rectangle which contains all the possible points which can be used as
        /// center for the joystick.
        /// </summary>
        private void SetJoystickCenterBounds()
        {
            RectTransform surfaceTransform = GetComponent<RectTransform>();
            Rect surface = surfaceTransform.rect;

            float dotHalfWidth = _joystickMovable.GetComponent<RectTransform>().rect.width / 2f;
            float movementRangeMargin = _movableJoystickControl.MovementRange + dotHalfWidth;

            float centerBoundsX = surface.x + (surface.width * m_JoystickCenterMargin.x / 2f) + movementRangeMargin;
            float centerBoundsY = surface.y + (surface.height * m_JoystickCenterMargin.y / 2f) + movementRangeMargin;
            float centerBoundsWidth = surface.width - (surface.width * m_JoystickCenterMargin.x)
                                                    - (movementRangeMargin * 2);
            float centerBoundsHeight = surface.height - (surface.height * m_JoystickCenterMargin.y)
                                                      - (movementRangeMargin * 2);
            _joystickCenterBounds = new Rect(centerBoundsX, centerBoundsY, centerBoundsWidth, centerBoundsHeight);
        }

        /// <summary>
        /// Move the joystick to the pressed position, and afterwards, pass the control to the joystick's movable dot.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            bool wasTouchPositionWithinCenterBounds = UpdateJoystickCenterPosition(eventData);

            if (wasTouchPositionWithinCenterBounds)
            {
                _movableJoystickControl.OnPointerDown(eventData);
            }
            else
            {
                _movableJoystickControl.OnDrag(eventData);
            }
        }

        /// <summary>
        /// Move the joystick according to the touched position.
        ///
        /// If the touch was not within the center bounds, it will proceed to update
        /// the joystick's pointer down position
        /// to the closest one within the center bounds' perimeter.
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns>Whether or not the touch was within the bounds for possible joystick's center points.</returns>
        private bool UpdateJoystickCenterPosition(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 touchPosition);

            bool isTouchPositionWithinCenterBounds = _joystickCenterBounds.Contains(touchPosition);

            if (isTouchPositionWithinCenterBounds)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    transform.parent.GetComponent<RectTransform>(),
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 rootTouchPosition);

                Vector3 rootPosition = new Vector3(rootTouchPosition.x, rootTouchPosition.y, 0f);

                m_Joystick.GetComponent<RectTransform>().localPosition = rootPosition;
            }
            else
            {
                Vector3 worldClosestPositionToCenterBounds =
                    GetWorldClosestPositionToCenterBoundsFromTouchPosition(touchPosition);

                Vector3 rootClosestPosition =
                    transform.parent.InverseTransformPoint(worldClosestPositionToCenterBounds);
                Vector3 rootPosition = new Vector3(rootClosestPosition.x, rootClosestPosition.y, 0f);

                m_Joystick.GetComponent<RectTransform>().localPosition = rootPosition;

                // Set pointer down pos relative to the closest point previously calculated.
                Vector2 joystickClosestPosition = m_Joystick
                    .transform
                    .InverseTransformPoint(worldClosestPositionToCenterBounds);

                _movableJoystickControl.PointerDownPos = joystickClosestPosition;
            }

            return isTouchPositionWithinCenterBounds;
        }

        private Vector3 GetWorldClosestPositionToCenterBoundsFromTouchPosition(Vector2 touchPosition)
        {
            CustomRectUtils rectUtils = new CustomRectUtils();
            Vector2 closestPoint = rectUtils.GetClosestPointFromARectPerimeter(touchPosition, _joystickCenterBounds);
            return transform.TransformPoint(closestPoint);
        }

        /// <summary>
        /// Transfer the event to the joystick's movable dot.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            _movableJoystickControl.OnPointerUp(eventData);
        }

        /// <summary>
        /// Transfer the event to the joystick's movable dot.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            _movableJoystickControl.OnDrag(eventData);
        }
    }
}