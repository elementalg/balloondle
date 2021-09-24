using System;
using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.UI
{
    /// <summary>
    /// Provides the ability to clamp the text of a <see cref="Text"/> component by using this component
    /// as a mean for editing the currently shown text within the <see cref="Text"/> component.
    ///
    /// It allows to set a custom amount of maximum characters allowed, and an end indicator for showing
    /// that the text has exceeded the maximum allowed amount.
    /// </summary>
    public class TextClamp : MonoBehaviour
    {
        [SerializeField, Tooltip("String attached to the end of the maximum amount of characters shown.")] 
        private string m_ClampedEndIndicator = "...";
        
        [SerializeField, Tooltip("Maximum amount of characters shown before clamping.")]
        private int m_MaxCharacters = 16;

        private Text _text;

        private void Start()
        {
            if (GetComponent<Text>() == null)
            {
                throw new InvalidOperationException("Missing Text component.");
            }

            _text = GetComponent<Text>();
        }

        /// <summary>
        /// Updates the text of the <see cref="Text"/>, assigning the text as passed, or a clamped version as configured
        /// within the Editor.
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            if (text.Length > m_MaxCharacters)
            {
                string clampedText = $"{text.Substring(0, m_MaxCharacters)}{m_ClampedEndIndicator}";

                _text.text = clampedText;
            }
            else
            {
                _text.text = text;
            }
        }
    }
}