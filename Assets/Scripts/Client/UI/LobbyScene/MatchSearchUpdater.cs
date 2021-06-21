using UnityEngine;

namespace Balloondle.Client
{
    /// <summary>
    /// Updates the visible UI in order to allow the logic actions depending upon if the player is looking for a
    /// match to play, or is going to use other options of the lobby scene.
    /// </summary>
    public class MatchSearchUpdater : MonoBehaviour
    {
        /// <summary>
        /// Play button which starts the search for a match.
        /// </summary>
        [SerializeField]
        private GameObject playButton;

        /// <summary>
        /// Text indicating that we are looking for a match.
        /// </summary>
        [SerializeField]
        private GameObject matchSearchStatus;

        /// <summary>
        /// Button used for cancelating the search.
        /// </summary>
        [SerializeField]
        private GameObject cancelButton;

        /// <summary>
        /// Hide the match search status at first.
        /// </summary>
        void Start()
        {
            HideMatchSearchStatus();    
        }

        /// <summary>
        /// Hides the play button, and shows the match status' text.
        /// </summary>
        public void ShowMatchSearchStatus()
        {
            playButton.SetActive(false);
            matchSearchStatus.SetActive(true);
            cancelButton.SetActive(true);
        }

        /// <summary>
        /// Hides the match status' text, and shows the play button.
        /// </summary>
        public void HideMatchSearchStatus()
        {
            playButton.SetActive(true);
            matchSearchStatus.SetActive(false);
            cancelButton.SetActive(false);
        }

        public void HideAll()
        {
            playButton.SetActive(false);
            matchSearchStatus.SetActive(false);
            cancelButton.SetActive(false);
        }
    }
}