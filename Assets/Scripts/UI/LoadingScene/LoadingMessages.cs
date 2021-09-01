using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.UI.LoadingScene
{
    /// <summary>
    ///     Changes the message displayed next to the loading circumference.
    /// </summary>
    public class LoadingMessages : MonoBehaviour
    {
        /// <summary>
        ///     Amount of time before the message is changed.
        /// </summary>
        [SerializeField] private float m_SecondsPerMessage = 2f;

        /// <summary>
        ///     UIElement containing the text that must be changed.
        /// </summary>
        [SerializeField] private Text m_Text;

        /// <summary>
        ///     Messages shown next to the loading circumference.
        /// </summary>
        private readonly string[] _messages =
        {
            "INFLATING BALLOONS...",
            "IMPORTING WEAPONS...",
            "FORGING KNIVES..."
        };

        /// <summary>
        ///     Number indicating which message is being shown.
        /// </summary>
        private int _messageIndex;

        /// <summary>
        ///     Amount of time elapsed since the last change.
        /// </summary>
        private float _timeAccumulator;

        /// <summary>
        ///     Calculate the time elapsed, and proceed to change the text
        ///     when the limit has been achieved.
        /// </summary>
        private void Update()
        {
            _timeAccumulator += Time.deltaTime;

            if (_timeAccumulator > m_SecondsPerMessage)
            {
                if (_messageIndex + 1 == _messages.Length)
                    _messageIndex = 0;
                else
                    _messageIndex += 1;

                m_Text.text = _messages[_messageIndex];
                _timeAccumulator = 0f;
            }
        }
    }
}