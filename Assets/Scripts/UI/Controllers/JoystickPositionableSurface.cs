using UnityEngine;

namespace Balloondle.UI.Controllers
{
    public class JoystickPositionableSurface : MonoBehaviourWithBoundsDetector
    {
        [SerializeField, Tooltip("GameObject containing the Joystick Range.")]
        private GameObject m_JoystickRange;

        [SerializeField] private RectTransform m_Bounds;

        [SerializeField, Tooltip("Horizontal, and vertical margins for the positionable surface.")]
        private Vector2 m_PositionableSurfaceMargin;

        private RectTransform _parentRectTransform;
        private Rect _positionableSurface;

        private void Start()
        {
            InitializePressDetector();

            _parentRectTransform = transform.parent.GetComponent<RectTransform>();

            Rect gameObjectSurface = _rectTransform.rect;

            Rect joystickRangeRect = m_JoystickRange.GetComponent<RectTransform>().rect;

            float movementMargin = joystickRangeRect.width * 2f / 3f;

            _positionableSurface = new Rect
            {
                x = gameObjectSurface.x + (m_PositionableSurfaceMargin.x * gameObjectSurface.width / 2f) 
                                        + movementMargin,
                y = gameObjectSurface.y + (m_PositionableSurfaceMargin.y * gameObjectSurface.height / 2f) 
                                        + movementMargin,
                width = gameObjectSurface.width -
                        (m_PositionableSurfaceMargin.x * gameObjectSurface.width) - (movementMargin * 2f),
                height = gameObjectSurface.height -
                         (m_PositionableSurfaceMargin.y * gameObjectSurface.height) - (movementMargin * 2f)
            };

            Rect parentRect = _parentRectTransform.rect;
            
            Vector2 anchorMax = _positionableSurface.max;
            anchorMax.x = (anchorMax.x - parentRect.x) / parentRect.width;
            anchorMax.y = (anchorMax.y - parentRect.y) / parentRect.height;

            Vector2 anchorMin = _positionableSurface.min;
            anchorMin.x = (anchorMin.x - parentRect.x) / parentRect.width;
            anchorMin.y = (anchorMin.y - parentRect.y) / parentRect.height;

            m_Bounds.anchorMax = anchorMax;
            m_Bounds.anchorMin = anchorMin;

            Debug.Log(m_Bounds);
        }

        public void OnPressed(Vector2 screenPoint)
        {
            // TODO: Fix detection.
            if (IsScreenPointInsidePositionableSurface(screenPoint)) 
            {
                RectTransformUtility
                    .ScreenPointToLocalPointInRectangle(_parentRectTransform, screenPoint,
                        null, out Vector2 localPoint);

                m_JoystickRange.transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
            }
            else
            {
                Vector2 closestWorldPoint = GetClosestPointFromPositionableSurface(screenPoint);
                m_JoystickRange.transform.localPosition = _parentRectTransform
                    .InverseTransformPoint(closestWorldPoint);
            }
        }

        private bool IsScreenPointInsidePositionableSurface(Vector2 screenPoint) 
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, screenPoint,
                null, out Vector2 localPoint);

            return _positionableSurface.Contains(localPoint);
        }

        private Vector2 GetClosestPointFromPositionableSurface(Vector2 inputPoint)
        {
            Vector2 closestPointFromPositionableSurface =
                CustomRectUtils.Instance.GetClosestPointFromARectPerimeter(inputPoint, _positionableSurface);
            
            return transform.TransformPoint(closestPointFromPositionableSurface);
        }  
    }
}