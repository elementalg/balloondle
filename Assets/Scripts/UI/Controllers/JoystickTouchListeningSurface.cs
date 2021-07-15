using System;
using Balloondle.Input;
using UnityEngine;
using Touch = Balloondle.Input.Touch;

namespace Balloondle.UI.Controllers
{
    public class JoystickTouchListeningSurface : MonoBehaviourWithBoundsDetector
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
        
        private ListeningState _listeningState;

        private void Start()
        {
            InitializePressDetector();
            
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
                TransferTouchBeginning(touch);

                _listeningState = ListeningState.UsingTouch;
                return;
            }

            m_Joystick.InputUpdate(touch.screenPosition);
            
            if (touch.HasEnded())
            {
                m_Joystick.InputEnd();
                
                HandleEndOfTouch();
            }
        }

        private void TransferTouchBeginning(Touch touch)
        {
            if (m_JoystickRange.IsScreenPointWithinBounds(touch.startScreenPosition))
            {
                m_Joystick.InputUpdate(touch.screenPosition);
            }
            else
            {
                m_JoystickPositionableSurface.OnPressed(touch.startScreenPosition);
            }
        }

        private bool HasTouchBegunWithinTheSurface(Touch touch)
        {
            return IsScreenPointWithinBounds(touch.startScreenPosition);
        }

        private void HandleEndOfTouch()
        {
            _listeningState = ListeningState.AwaitingTouch;
            
            ListenForTouchesWithinTheSurface();
        }
    }
}