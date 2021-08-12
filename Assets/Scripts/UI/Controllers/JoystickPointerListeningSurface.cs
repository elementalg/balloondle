﻿using System;
using Balloondle.Input;
using UnityEngine;
using UnityEngine.UI;

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
        private Image m_JoystickRangeCenter;

        [SerializeField] 
        private Joystick m_Joystick;
        
        private ListeningState _listeningState;

        private JoystickAlphaSetter _joystickAlphaSetter;
        private bool _isAlphaSetterAvailable;

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

            _joystickAlphaSetter = GetComponent<JoystickAlphaSetter>();
            _isAlphaSetterAvailable = _joystickAlphaSetter != null;

            if (_isAlphaSetterAvailable)
            {
                InitializeAlphaSetter();
            }
            
            ListenForPointerWithinTheSurface();
        }

        /// <summary>
        /// To be called only once at the start of the script.
        /// It proceeds to assign the joystick's <see cref="UnityEngine.UI.Image"/>
        /// components to the <see cref="JoystickAlphaSetter"/>.
        /// </summary>
        private void InitializeAlphaSetter()
        {
            if (m_JoystickRange.GetComponent<Image>() == null)
            {
                throw new InvalidOperationException(
                    "JoystickRange requires an Image component in order to initialize the alpha setter");
            }
            
            if (m_JoystickRangeCenter == null)
            {
                throw new InvalidOperationException("Missing Image component for JoystickRangeCenter.");
            }

            if (m_Joystick.GetComponent<Image>() == null)
            {
                throw new InvalidOperationException(
                    "Joystick requires an Image component in order to initialize the alpha setter.");
            }
            
            _joystickAlphaSetter.joystickRange = m_JoystickRange.GetComponent<Image>();
            _joystickAlphaSetter.joystickRangeCenter = m_JoystickRangeCenter;
            _joystickAlphaSetter.joystick = m_Joystick.GetComponent<Image>();
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

            if (_isAlphaSetterAvailable)
            {
                _joystickAlphaSetter.OnJoystickSelected();
            }
        }

        private bool HasTouchBegunWithinTheSurface(IPointerPress pointer)
        {
            return IsScreenPointWithinBounds(pointer.startScreenPosition);
        }

        private void HandleEndOfPointer()
        {
            _listeningState = ListeningState.AwaitingPointer;

            if (_isAlphaSetterAvailable)
            {
                _joystickAlphaSetter.OnJoystickDeselected();
            }
            
            ListenForPointerWithinTheSurface();
        }
    }
}