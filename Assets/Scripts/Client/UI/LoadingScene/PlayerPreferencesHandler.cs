using Balloondle.Shared.Net.Models;
using System;
using UnityEngine;

namespace Balloondle.Client.UI.LoadingScene
{
    /// <summary>
    /// Provides a centralized and managed interface to Unity's player preferences, in order to
    /// make easier the process of storing and retrieving important game related data.
    /// </summary>
    public class PlayerPreferencesHandler : MonoBehaviour
    {
        private const string NAME_KEY = "name";
        private const string CODE_KEY = "code";

        /// <summary>
        /// Checks whether or not the authentication details are stored on the player's preferences.
        /// </summary>
        /// <returns>True if the authentication details are stored, false otherwise.</returns>
        public bool HasRequiredAuthenticationDetails()
        {
            return PlayerPrefs.HasKey(NAME_KEY) && PlayerPrefs.HasKey(CODE_KEY);
        }

        /// <summary>
        /// Proceeds to store the authentication details into the player's preferences.
        /// </summary>
        /// <param name="name">Name of the player.</param>
        /// <param name="code">Code used for identifying the player.</param>
        public void StoreAuthenticationDetails(string name, UInt32 code)
        {
            PlayerPrefs.SetString(NAME_KEY, name);
            PlayerPrefs.SetString(CODE_KEY, code.ToString());
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Obtain the user instance for the authentication details stored within the player's
        /// preferences.
        /// </summary>
        /// <returns>An user instance containing the authentication details</returns>
        /// <exception cref="InvalidOperationException">If player's preferences does not contain 
        /// the authentication details.</exception>
        public User GetUserFromAuthenticationDetails()
        {
            if (!HasRequiredAuthenticationDetails())
            {
                throw new InvalidOperationException("Authentication details are not available.");
            }

            User user = new User();
            user.name = PlayerPrefs.GetString(NAME_KEY);
            user.code = UInt32.Parse(PlayerPrefs.GetString(CODE_KEY));

            return user;
        }
    }
}