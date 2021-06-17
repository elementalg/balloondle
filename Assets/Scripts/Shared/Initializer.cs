using MLAPI;
using UnityEngine;

namespace Balloondle.Shared
{
    /// <summary>
    /// Initializes the server or client depending upon the configuration specified
    /// on the scene.
    /// </summary>
    public class Initializer : MonoBehaviour
    {
        [SerializeField]
        private GameObject serverLogicPrefab;

        [SerializeField]
        private GameObject clientLogicPrefab;

        [SerializeField]
        private bool isServer;

        // Start is called before the first frame update
        void Start()
        {
            if (isServer)
            {
                Instantiate(serverLogicPrefab);
            } 
            else
            {
                Instantiate(clientLogicPrefab);
            }
        }
    }
}
