using System;
using UnityEngine;

namespace Balloondle.UI
{
    public abstract class MonoBehaviourWithBoundsDetector : MonoBehaviour
    {
        private RectTransform _rectTransform;

        protected void InitializePressDetector()
        {
            if (GetComponent<RectTransform>() == null)
            {
                throw new InvalidOperationException(
                    "MonoBehaviourWithBoundsDetector's GameObject must have a RectTransform.");
            }

            _rectTransform = GetComponent<RectTransform>();
        }

        public bool IsScreenPointWithinBounds(Vector2 screenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, screenPoint);
        }
    }
}