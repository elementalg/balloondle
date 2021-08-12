using System;
using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.UI.Controllers
{
    public class JoystickAlphaSetter : MonoBehaviour
    {
        [SerializeField, Tooltip("X -> Alpha on selected, Y -> Alpha on deselected")]
        private Vector2 m_JoystickRangeAlpha = new Vector2(1.0f, 0.5f);

        [SerializeField, Tooltip("X -> Alpha on selected, Y -> Alpha on deselected")]
        private Vector2 m_JoystickRangeCenterAlpha = new Vector2(1.0f, 0.0f);

        [SerializeField, Tooltip("X -> Alpha on selected, Y -> Alpha on deselected")]
        private Vector2 m_JoystickAlpha = new Vector2(1.0f, 0.75f);
        
        public Image joystickRange { get; set; }
        public Image joystickRangeCenter { get; set; }
        public Image joystick { get; set; }

        public void OnJoystickSelected()
        {
            SetImageAlpha(joystickRange, m_JoystickRangeAlpha.x);
            SetImageAlpha(joystickRangeCenter, m_JoystickRangeCenterAlpha.x);
            SetImageAlpha(joystick, m_JoystickAlpha.x);
        }

        public void OnJoystickDeselected()
        {
            SetImageAlpha(joystickRange, m_JoystickRangeAlpha.y);
            SetImageAlpha(joystickRangeCenter, m_JoystickRangeCenterAlpha.y);
            SetImageAlpha(joystick, m_JoystickAlpha.y);
        }
        
        private static void SetImageAlpha(Image image, float alpha)
        {
            Color imageColor = image.color;
            image.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
        }
    }
}