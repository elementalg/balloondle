using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.Client
{
    /// <summary>
    /// Create a cache on the player's device storing the name inserted through
    /// the input field.
    /// </summary>
    public class GeneratePreferences : MonoBehaviour
    {
        /// <summary>
        /// Save the name from the input field whenever the player presses the button.
        /// </summary>
        public void OnEnterStartupForm()
        {
            GameObject inputNameField = GameObject.Find("NameField");
            InputField inputField = inputNameField.GetComponent<InputField>();

            if (inputField.text.Length > 0)
            {
                PlayerPrefs.SetString("name", inputField.text);

                PreferencesCheckerFunctionality preferencesChecker = GameObject
                    .Find("PreferencesChecker")
                    .GetComponent<PreferencesCheckerFunctionality>();

                preferencesChecker.CheckPreferences();
            }
        }
    }
}
