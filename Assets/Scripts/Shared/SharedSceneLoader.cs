using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Shared
{
    /// <summary>
    /// Loads the scene containing the shared objects in an additive way, thus keeping the server's or
    /// client's scene untouched.
    /// </summary>
    public class SharedSceneLoader : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene(Scenes.GameSharedScene.ToString(), LoadSceneMode.Additive);
        }

        void Update()
        {
        
        }
    }
}
