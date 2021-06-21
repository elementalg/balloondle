using Balloondle.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Client
{
    public class BackToLobbyScene : MonoBehaviour
    {
        public void GoToLobbyScene()
        {
            // Switch to the lobby scene by loading the lobby scene in single mode.
            SceneManager.LoadScene(Scenes.LobbyScene.ToString(), LoadSceneMode.Single);
        }
    }
}