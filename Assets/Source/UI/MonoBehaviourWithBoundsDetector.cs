using System;
using UnityEngine;

namespace Balloondle.UI
{
    public abstract class MonoBehaviourWithBoundsDetector : MonoBehaviour
    {
        protected RectTransform RectTransform;

        protected void InitializePressDetector()
        {
            if (GetComponent<RectTransform>() == null)
            {
                throw new InvalidOperationException(
                    "MonoBehaviourWithBoundsDetector's GameObject must have a RectTransform.");
            }

            RectTransform = GetComponent<RectTransform>();
        }

        public virtual bool IsScreenPointWithinBounds(Vector2 screenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(RectTransform, screenPoint);
        }
    }
}