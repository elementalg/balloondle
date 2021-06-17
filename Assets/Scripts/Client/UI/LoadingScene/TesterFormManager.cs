using Balloondle.Client.UI.LoadingScene;
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
        private const string PLAYER_PREFERENCES_HANDLER = "Player Preferences Handler";

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
            GameObject playerPreferencesHandler = GameObject.Find(PLAYER_PREFERENCES_HANDLER);
            PlayerPreferencesHandler preferences = playerPreferencesHandler
                .GetComponent<PlayerPreferencesHandler>();

            return preferences.HasRequiredAuthenticationDetails();
        }

        /// <summary>
        /// Sets the visibility of the tester's authentication form.
        /// </summary>
        /// <param name="visibility">True to make the form visible, false otherwise.</param>
        private void SetTesterFormVisible(bool visibility)
        {
            // All UI elements used for the tester's form, have the tag "TesterForm".
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(TESTER_FORM_TAG);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(visibility);
            }
        }
    }
}