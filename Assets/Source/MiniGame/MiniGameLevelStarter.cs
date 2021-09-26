using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.MiniGame
{
    public class MiniGameLevelStarter : MonoBehaviour
    {
        public void StartMiniGameScene()
        {
            SceneManager.LoadScene(SceneNames.MiniGame);
        }
    }
}