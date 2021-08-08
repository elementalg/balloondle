using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Balloondle.Input
{
    public class MouseListener : MonoBehaviour
    {
        public Action<MouseClick> OnMouseClickUpdate;

        private MouseClick _mouseClickLeft;
        private MouseClick _mouseClickMiddle;
        private MouseClick _mouseClickRight;

        // Start is called before the first frame update
        void Start()
        {
            _mouseClickLeft = new MouseClick
            {
                pointerId = MouseClick.MouseLeftClickButtonPointerID,
                pointerPhase = PointerPhase.None
            };

            _mouseClickMiddle = new MouseClick
            {
                pointerId = MouseClick.MouseMiddleClickButtonPointerID,
                pointerPhase = PointerPhase.None
            };

            _mouseClickRight = new MouseClick
            {
                pointerId = MouseClick.MouseRightClickButtonPointerID,
                pointerPhase = PointerPhase.None
            };
        }

        // Update is called once per frame
        void Update()
        {
            Mouse mouse = Mouse.current;
            
            if (mouse == null) 
            {
                return;
            }

            for (int i = -1; i >= MouseClick.MouseRightClickButtonPointerID; i--)
            {
                MouseClick currentMouseClick = i switch
                {
                    MouseClick.MouseLeftClickButtonPointerID => _mouseClickLeft,
                    MouseClick.MouseMiddleClickButtonPointerID => _mouseClickMiddle,
                    MouseClick.MouseRightClickButtonPointerID => _mouseClickRight,
                    _ => throw new InvalidOperationException("Unknown current mouse click detected."),
                };

                UpdateMouseClickContainerByMouseButtonPointerId(mouse, currentMouseClick);

                if (currentMouseClick.pointerPhase != PointerPhase.None)
                {
                    OnMouseClickUpdate?.Invoke(currentMouseClick);
                }
            }
        }

        private void UpdateMouseClickContainerByMouseButtonPointerId(Mouse mouse, MouseClick currentMouseClick)
        {
            ButtonControl mouseButton = currentMouseClick.pointerId switch
            {
                MouseClick.MouseLeftClickButtonPointerID => mouse.leftButton,
                MouseClick.MouseMiddleClickButtonPointerID => mouse.middleButton,
                MouseClick.MouseRightClickButtonPointerID => mouse.rightButton,
                _ => throw new InvalidOperationException("Unknown mouse pointer id detected."),
            };

            if (mouseButton.wasPressedThisFrame)
            {
                OnMouseButtonStart(mouse, currentMouseClick);
                return;
            }
            else if (mouseButton.wasReleasedThisFrame)
            {
                OnMouseButtonEnd(currentMouseClick);
                return;
            }

            if (mouseButton.isPressed)
            {
                if (Mathf.Approximately(0f, (float)currentMouseClick.startTime))
                {
                    OnMouseButtonStart(mouse, currentMouseClick);
                }
                else
                {
                    OnMouseButtonMove(mouse, currentMouseClick);
                }
            }
            else
            {
                OnMouseButtonEnd(currentMouseClick);
            }
        }

        private void OnMouseButtonStart(Mouse mouse, MouseClick mouseClick)
        {
            mouseClick.pointerPhase = PointerPhase.Began;
            mouseClick.startTime = Time.realtimeSinceStartupAsDouble;
            mouseClick.startScreenPosition = mouse.position.ReadValue();
            mouseClick.screenPosition = mouse.position.ReadValue();
        }

        private void OnMouseButtonMove(Mouse mouse, MouseClick mouseClick)
        {
            mouseClick.pointerPhase = PointerPhase.Moved;
            mouseClick.screenPosition = mouse.position.ReadValue();
        }

        private void OnMouseButtonEnd(MouseClick mouseClick)
        {
            mouseClick.startTime = 0.0;
            
            if (mouseClick.pointerPhase == PointerPhase.Ended)
            {
                mouseClick.pointerPhase = PointerPhase.None;
                return;
            }

            if (mouseClick.pointerPhase != PointerPhase.None)
            {
                mouseClick.pointerPhase = PointerPhase.Ended;
            }
        }
    }
}