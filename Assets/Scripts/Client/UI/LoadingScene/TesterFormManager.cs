using UnityEngine;

namespace Balloondle.Client
{
    /// <summary>
    /// Handles the visibility of the tester form. 
    /// 
    /// Showing the tester's form only if the player's preferences does not contain any
    /// name and code details, used to authenticate testers on the lobby.
    /// </summary>
    public class TesterFormManager : MonoBehaviour
    {
        /// <summary>
        /// Key used for storing the tester's name in the player's preferences.
        /// </summary>
        private const string NAME_PREFERENCE_KEY = "name";
        /// <summary>
        /// Key used for storing the tester's code in the player's preferences.
        /// </summary>
        private const string CODE_PREFERENCE_KEY = "code";

        /// <summary>
        /// Tag which every UI element takes part in the tester's form.
        /// </summary>
        private const string TESTER_FORM_TAG = "TesterForm";

        /// <summary>
        /// Check for authentication details in the player's preferences, if there are not any
        /// authentication details, proceed to show the tester's authentication form.
        /// </summary>
        void Start()
        {
            if (AreTestersAuthenticationDetailsAvailable())
            {
                SetTesterFormVisible(false);
                return;
            }

            SetTesterFormVisible(true);
        }

        /// <summary>
        /// Look after, authentication details of the tester, on the player's preferences.
        /// </summary>
        /// <returns>True if player has "name" and "code" keys on player's preferences, false
        /// if otherwise.</returns>
        private bool AreTestersAuthenticationDetailsAvailable()
        {
            return PlayerPrefs.HasKey("name") && PlayerPrefs.HasKey("code");
        }

        /// <summary>
        /// Sets the visibility of the tester's authentication form.
        /// </summary>
        /// <param name="visibility">True to make the form visible, false otherwise.</param>
        private void SetTesterFormVisible(bool visibility)
        {
            // All UI elements used for the tester's form, have the tag "TesterForm".
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("TesterForm");

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(visibility);
            }
        }
    }
}