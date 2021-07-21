using System;
using UnityEngine;

namespace Balloondle.Input
{
    public class PointerListener : MonoBehaviour
    {
        [SerializeField, Tooltip("Whether or not to listen to the touchscreen.")]
        private bool m_ListenToTouchscreen;

        [SerializeField, Tooltip("Whether or not to listen to the mouse.")]
        private bool m_ListenToMouse;

        public Action<IPointerPress> OnPointerUpdate;

        void Start()
        {
            if (m_ListenToTouchscreen)
            {
                TouchListener touchListener = gameObject.AddComponent<TouchListener>();
                touchListener.OnTouchUpdate += OnPointerUpdate;
            }

            if (m_ListenToMouse)
            {
                MouseListener mouseListener = gameObject.AddComponent<MouseListener>();
                mouseListener.OnMouseClickUpdate += OnPointerUpdate;
            }
        }
    }
}
