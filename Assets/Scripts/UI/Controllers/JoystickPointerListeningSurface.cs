using System;
using Balloondle.Input;
using UnityEngine;

namespace Balloondle.UI.Controllers
{
    public class JoystickPointerListeningSurface : MonoBehaviourWithBoundsDetector
    {
        private enum ListeningState
        {
            AwaitingPointer,
            UsingPointer,
        }

        [SerializeField, Tooltip("Touch Demultiplexer used in the current scene.")]
        private PointerDemultiplexerBehaviour m_TouchDemultiplexerBehaviour;

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

            ListenForPointerWithinTheSurface();
        }

        private void ListenForPointerWithinTheSurface()
        {
            // Listen for pointers which are within the surface.
            m_TouchDemultiplexerBehaviour
                .Demultiplexer
                .AddOutputToQueue(OnPointerUpdate, HasTouchBegunWithinTheSurface);
        }

        private void OnPointerUpdate(IPointerPress pointer)
        {
            if (pointer.HasEnded())
            {
                m_Joystick.InputEnd();
                
                HandleEndOfPointer();
                return;
            }

            if (_listeningState == ListeningState.AwaitingPointer)
            {
                TransferTouchBeginning(pointer);

                _listeningState = ListeningState.UsingPointer;
            }

            m_Joystick.InputUpdate(pointer.screenPosition);
        }

        private void TransferTouchBeginning(IPointerPress pointer)
        {
            if (m_JoystickRange.IsScreenPointWithinBounds(pointer.startScreenPosition))
            {
                m_Joystick.InputUpdate(pointer.screenPosition);
            }
            else
            {
                m_JoystickPositionableSurface.OnPressed(pointer.startScreenPosition);
            }
        }

        private bool HasTouchBegunWithinTheSurface(IPointerPress pointer)
        {
            return IsScreenPointWithinBounds(pointer.startScreenPosition);
        }

        private void HandleEndOfPointer()
        {
            _listeningState = ListeningState.AwaitingPointer;
            
            ListenForPointerWithinTheSurface();
        }
    }
}