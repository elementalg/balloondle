using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Balloondle.Input
{
    public class PointerListener : MonoBehaviour
    {
        [SerializeField, Tooltip("Whether or not to listen to the touchscreen.")]
        private bool m_ListenToTouchscreen;

        [SerializeField, Tooltip("Whether or not to listen to the mouse.")]
        private bool m_ListenToMouse;

        [CanBeNull] private TouchListener _touchListener;
        [CanBeNull] private MouseListener _mouseListener;

        [CanBeNull] private Action<IPointerPress> _onPointerUpdate;
        
        public Action<IPointerPress> OnPointerUpdate
        {
            get => _onPointerUpdate;
            set
            {
                if (m_ListenToTouchscreen)
                {
                    if (_touchListener is null)
                    {
                        return;
                    }
                    
                    _touchListener.OnTouchUpdate += value;
                }

                if (m_ListenToMouse)
                {
                    if (_mouseListener is null)
                    {
                        return;
                    }

                    _mouseListener.OnMouseClickUpdate += value;
                }

                _onPointerUpdate = value;
            }
        }

        private void Start()
        {
            if (m_ListenToTouchscreen)
            {
                _touchListener = gameObject.AddComponent<TouchListener>();
                _touchListener.OnTouchUpdate += OnPointerUpdate;
            }

            if (m_ListenToMouse)
            {
                _mouseListener = gameObject.AddComponent<MouseListener>();
                _mouseListener.OnMouseClickUpdate += OnPointerUpdate;
            }
        }
    }
}
