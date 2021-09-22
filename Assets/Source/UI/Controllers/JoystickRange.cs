using UnityEngine;

namespace Balloondle.UI.Controllers
{
    public class JoystickRange : MonoBehaviourWithBoundsDetector
    {
        private float _rangeRadius;

        private void Start()
        {
            InitializePressDetector();

            // JoystickRange is expected to be a circle, thus the width and the height must be the same.
            _rangeRadius = RectTransform.rect.width / 2f;
        }

        public override bool IsScreenPointWithinBounds(Vector2 screenPoint)
        {
            RectTransformUtility
                .ScreenPointToLocalPointInRectangle(RectTransform, screenPoint, null, out Vector2 localPoint);

            // Screen point's distance from the range's center (0,0), must not be greather than the range's radius.
            return localPoint.magnitude <= _rangeRadius;
        }
    }
}