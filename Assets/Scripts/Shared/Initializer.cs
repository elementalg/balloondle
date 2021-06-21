using MLAPI;
using MLAPI.Transports.UNET;
using UnityEditor;
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
                string ip = PlayerPrefs.GetString("ip");
                int port = PlayerPrefs.GetInt("port");

                Debug.Log($"Starting: {ip}:{port}");
                GameObject networkManger = GameObject.Find("NetworkManager");
                UNetTransport transport = networkManger.GetComponent<UNetTransport>();

                transport.ConnectAddress = ip;
                transport.ConnectPort = port;

                PlayerPrefs.DeleteKey("ip");
                PlayerPrefs.DeleteKey("port");
                PlayerPrefs.Save();

                Instantiate(clientLogicPrefab);
            }
        }
    }
}
