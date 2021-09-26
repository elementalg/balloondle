using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle
{
    public class OnEndFirstRun : MonoBehaviour
    {
        public void OnPlayButtonClick()
        {
            SceneManager.LoadScene(SceneNames.Loading, LoadSceneMode.Single);
        }
    }
}