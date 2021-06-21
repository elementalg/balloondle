using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Client
{
    /// <summary>
    /// Provides the logic for what happens after the prerequirements for the loading of scenes
    /// has been met.
    /// </summary>
    public class LoadingManager : MonoBehaviour
    {
        /// <summary>
        /// Name used within the Editor for the scene containing the loading's logic and visual elements.
        /// </summary>
        private const string LOADING_SCENE = "LoadingScene";

        /// <summary>
        /// Name used within the Editor for the scene containing the lobby's logic and visual elements.
        /// </summary>
        private const string LOBBY_SCENE = "LobbyScene";

        /// <summary>
        /// Proceed to load the lobby scene when the prerequirements, which the loading manager does
        /// not need to know about, are met.
        /// </summary>
        public void OnPreparedToLoad()
        {
            // Destroy this scene's event system, in order to avoid bugging the Lobby's one.
            GameObject eventSystem = GameObject.Find("EventSystem");
            Destroy(eventSystem);

            AsyncOperation operation = SceneManager.LoadSceneAsync(LOBBY_SCENE, LoadSceneMode.Additive);
            operation.completed += OnLobbySceneHasBeenLoaded;
        }

        /// <summary>
        /// Triggered when the loading of the lobby scene has been completed.
        /// </summary>
        /// <param name="operation"></param>
        public void OnLobbySceneHasBeenLoaded(AsyncOperation operation)
        {
            GameObject lobbyCommunicator = GameObject.Find("Lobby Communicator");
            LobbyCommunicator communicator = lobbyCommunicator.GetComponent<LobbyCommunicator>();

            // Pass the required details to the lobby scene.
            communicator.SendDetailsToLobbyScene();

            // Set the lobby scene as active, before unloading the loading scene.
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(LOBBY_SCENE));

            // Proceed to unload the loading scene, since we are now on the lobby scene.
            SceneManager.UnloadSceneAsync(LOADING_SCENE);
        }
    }
}