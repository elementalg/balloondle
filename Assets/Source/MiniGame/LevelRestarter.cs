using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.MiniGame
{
    public class LevelRestarter : MonoBehaviour
    {
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneNames.MiniGame);
        }
    }
}