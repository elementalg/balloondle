using System;
using UnityEngine;

namespace Balloondle.UI
{
    public class Joystick : MonoBehaviour
    {
        private RectTransform _parentRectTransform;
        private Rect _rect;

        public void Start()
        {
            if (transform.parent.GetComponent<RectTransform>() == null)
            {
                throw new InvalidOperationException("Joystick requires parent to have a RectTransform.");
            }

            _parentRectTransform = transform.parent.GetComponent<RectTransform>();

            if (GetComponent<RectTransform>()?.rect == null)
            {
                throw new InvalidOperationException("Joystick requires GameObject to have a RectTransform.");
            }
            
            _rect = GetComponent<RectTransform>().rect;
        }
        
        public bool IsScreenPointWithinBounds(Vector2 screenPoint, Camera gameCamera)
        {
            return CustomRectUtils.Instance.IsScreenPointWithinRect(
                screenPoint,
                gameCamera,
                _parentRectTransform,
                _rect);
        }
    }
}