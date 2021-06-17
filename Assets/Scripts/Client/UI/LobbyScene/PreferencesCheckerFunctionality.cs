using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Client
{
    /// <summary>
    /// Manages what UI elements are shown depending upon the state of the
    /// preferences.
    /// </summary>
    public class PreferencesCheckerFunctionality : MonoBehaviour
    {
        /// <summary>
        /// Check the preferences directly on the start.
        /// </summary>
        void Start()
        {
            CheckPreferences();
        }

        /// <summary>
        /// Hide the start up form if there is data on the preferences. Otherwise,
        /// show the form, and hide the play button.
        /// </summary>
        public void CheckPreferences()
        {
            // If player has the key 'name',
            // it means this player has already played the game.
            if (PlayerPrefs.HasKey("name"))
            {
                DestroyStartupForm();
                ShowPlayButton();
            }
            else
            {
                HidePlayButton();
            }
        }

        /// <summary>
        /// Removes the start up form from the Scene.
        /// </summary>
        private void DestroyStartupForm()
        {
            GameObject[] startupForm = GameObject.FindGameObjectsWithTag("StartupForm");

            foreach (GameObject uiElement in startupForm)
            {
                Destroy(uiElement);
            }
        }

        /// <summary>
        /// Makes the PlayButton visible.
        /// </summary>
        private void ShowPlayButton()
        {
            GameObject playButton = GameObject.Find("PlayButton");

            playButton.SetActive(true);
        }

        /// <summary>
        /// Hides the PlayButton.
        /// </summary>
        private void HidePlayButton()
        {
            GameObject playButton = GameObject.Find("PlayButton");

            playButton.SetActive(false);
        }
    }
}
