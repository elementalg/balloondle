using System;
using Balloondle.Input;
using UnityEngine;
using Touch = Balloondle.Input.Touch;

namespace Balloondle.UI.Controllers
{
    public class JoystickTouchListeningSurface : MonoBehaviour
    {
        private enum ListeningState
        {
            AwaitingTouch,
            UsingTouch,
        }

        [SerializeField, Tooltip("Touch Demultiplexer used in the current scene.")]
        private TouchDemultiplexerBehaviour m_TouchDemultiplexerBehaviour;

        [SerializeField] 
        private JoystickPositionableSurface m_JoystickPositionableSurface;

        [SerializeField] 
        private JoystickRange m_JoystickRange;

        [SerializeField] 
        private Joystick m_Joystick;

        private RectTransform _rectTransform;

        private ListeningState _listeningState;

        private void Start()
        {
            if (m_TouchDemultiplexerBehaviour == null)
            {
                throw new InvalidOperationException("Joystick's touch listening surface requires " +
                                                    "an instance of TouchDemultiplexerBehaviour to be assigned.");
            }

            if (m_JoystickRange == null)
            {
                throw new InvalidOperationException(
                    "Joystick's touch listening surface requires an instance of JoystickRange to be assigned.");
            }

            if (m_Joystick == null)
            {
                throw new InvalidOperationException(
                    "Joystick's touch listening surface requires an instance of Joystick to be assigned.");
            }

            if (transform.parent.GetComponent<RectTransform>() == null)
            {
                throw new InvalidOperationException(
                    "Joystick's touch listening surface must be a child of a Canvas.");
            }

            _rectTransform = GetComponent<RectTransform>();

            ListenForTouchesWithinTheSurface();
        }

        private void ListenForTouchesWithinTheSurface()
        {
            // Listen for touches which are within the surface.
            m_TouchDemultiplexerBehaviour.Demultiplexer.AddOutputToQueue(OnTouchUpdate, HasTouchBegunWithinTheSurface);
        }

        private void OnTouchUpdate(Touch touch)
        {
            // TODO: Transfer touch to the element which contains the touch.

            if (_listeningState == ListeningState.AwaitingTouch)
            {
                Debug.Log($"[JOYSTICK-TOUCH-LISTENING-SURFACE] Touch started: {touch.startScreenPosition}");
                TransferTouchBeginning(touch);

                _listeningState = ListeningState.UsingTouch;
                return;
            }

            Debug.Log($"[JOYSTICK-TOUCH-LISTENING-SURFACE] Touch update: {touch.screenPosition}");
            m_Joystick.InputUpdate(touch.screenPosition);
            
            if (touch.HasEnded())
            {
                Debug.Log("[JOYSTICK-TOUCH-LISTENING-SURFACE] Touch ended.");
                HandleEndOfTouch();
            }
        }

        private void TransferTouchBeginning(Touch touch)
        {
            if (m_Joystick.IsScreenPointWithinJoystick(touch.startScreenPosition))
            {
                m_Joystick.OnPressed(touch.startScreenPosition);
            }
        }

        private bool HasTouchBegunWithinTheSurface(Touch touch)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, touch.startScreenPosition);
        }

        private void HandleEndOfTouch()
        {
            _listeningState = ListeningState.AwaitingTouch;
            
            ListenForTouchesWithinTheSurface();
        }
    }
}