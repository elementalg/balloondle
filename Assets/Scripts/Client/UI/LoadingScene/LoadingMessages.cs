using System;
using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.Client
{
    /// <summary>
    /// Changes the message displayed next to the loading circumference.
    /// </summary>
    public class LoadingMessages : MonoBehaviour
    {
        /// <summary>
        /// Amount of time before the message is changed.
        /// </summary>
        [SerializeField]
        private float secondsPerMessage = 2f;

        /// <summary>
        /// UIElement containing the text that must be changed.
        /// </summary>
        [SerializeField]
        private Text text;

        /// <summary>
        /// Messages shown next to the loading circumference.
        /// </summary>
        private readonly string[] messages = new string[]
        {
            "INFLATING BALLOONS...",
            "IMPORTING WEAPONS...",
            "FORGING KNIVES..."
        };

        /// <summary>
        /// Number indicating which message is being shown.
        /// </summary>
        private int messageIndex = 0;

        /// <summary>
        /// Amount of time elapsed since the last change.
        /// </summary>
        private float timeAccumulator = 0f;

        /// <summary>
        /// Calculate the time elapsed, and proceed to change the text
        /// when the limit has been achieved.
        /// </summary>
        void Update()
        {
            timeAccumulator += Time.deltaTime;

            if (timeAccumulator > secondsPerMessage)
            {
                if (messageIndex + 1 == messages.Length)
                {
                    messageIndex = 0;
                }
                else
                {
                    messageIndex += 1;
                }

                text.text = messages[messageIndex];
                timeAccumulator = 0f;
            }
        }
    }
}
