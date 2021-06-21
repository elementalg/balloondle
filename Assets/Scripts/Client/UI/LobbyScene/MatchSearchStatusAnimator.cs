using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.Client
{
    /// <summary>
    /// Adds a dot each second until the limit is achieved, starting then with no dots.
    /// </summary>
    public class MatchSearchStatusAnimator : MonoBehaviour
    {
        private const string DOTS = "...";

        private Text text;
        private int lap = 0;
        private string originalText;

        /// <summary>
        /// Append or remove '.' as required.
        /// </summary>
        void OnEnable()
        {
            text = GetComponent<Text>();
            // Make a copy of the original text.
            originalText = string.Copy(text.text);

            StartCoroutine(AnimateText());
        }

        /// <summary>
        /// Add one more dot each second, until the maximum is achieved, thus beginning from 0 again.
        /// </summary>
        /// <returns></returns>
        IEnumerator AnimateText()
        {
            while (true)
            {
                lap = (lap + 1 > DOTS.Length) ? 0 : lap + 1;

                text.text = originalText + DOTS.Substring(0, lap);

                // Wait one second until adding a new dot.
                yield return new WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// Stop the coroutine when the Match Search Status text is disabled.
        /// </summary>
        void OnDisable()
        {
            StopCoroutine(AnimateText());
        }
    }
}