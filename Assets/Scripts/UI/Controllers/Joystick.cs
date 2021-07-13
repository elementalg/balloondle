using System;
using UnityEngine;

namespace Balloondle.UI.Controllers
{
    public class Joystick : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Start()
        {
            if (GetComponent<RectTransform>() == null)
            {
                throw new InvalidOperationException("Joystick's GameObject must have a RectTransform.");
            }

            _rectTransform = GetComponent<RectTransform>();
        }

        public bool IsScreenPointWithinJoystick(Vector2 screenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, screenPoint);
        }

        public void OnPressed(Vector2 screenPoint)
        {
            Debug.Log("[JOYSTICK] OnPressed");
        }

        public void InputUpdate(Vector2 screenPoint)
        {
            Debug.Log("[JOYSTICK] OnDrag");
        }
    }
}