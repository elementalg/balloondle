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
        
        public Image JoystickRange { get; set; }
        public Image JoystickRangeCenter { get; set; }
        public Image Joystick { get; set; }

        public void OnJoystickSelected()
        {
            SetImageAlpha(JoystickRange, m_JoystickRangeAlpha.x);
            SetImageAlpha(JoystickRangeCenter, m_JoystickRangeCenterAlpha.x);
            SetImageAlpha(Joystick, m_JoystickAlpha.x);
        }

        public void OnJoystickDeselected()
        {
            SetImageAlpha(JoystickRange, m_JoystickRangeAlpha.y);
            SetImageAlpha(JoystickRangeCenter, m_JoystickRangeCenterAlpha.y);
            SetImageAlpha(Joystick, m_JoystickAlpha.y);
        }
        
        private static void SetImageAlpha(Image image, float alpha)
        {
            Color imageColor = image.color;
            image.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
        }
    }
}