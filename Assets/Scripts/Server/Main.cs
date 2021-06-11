using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine.SceneManagement;
using Balloondle.Shared;

namespace Balloondle.Server
{
    /// <summary>
    /// Class to be appended with a root object for the scene containing the game server's logic, so
    /// the server starts listening for incoming connections as specified by the incoming command line
    /// arguments.
    /// </summary>
    public class Main : MonoBehaviour
    {
        private const int EXPECTED_ARGUMENTS_INCLUDING_BINARY = 3;

        void Start()
        {
            Debug.Log("- Starting.");

            SceneManager.sceneLoaded += OnSharedSceneHasBeenLoaded;
        }

        void OnSharedSceneHasBeenLoaded(Scene loadedScene, LoadSceneMode mode)
        {
            if (loadedScene.name.Equals(Scenes.GameSharedScene.ToString()))
            {
                Debug.Log("- Preparing to listen to incoming connections.");

                CommandLineArgumentsParser parser = new CommandLineArgumentsParser();
                Dictionary<string, string> startArguments = parser
                    .GetExpectedCommandLineArguments(System.Environment.GetCommandLineArgs(),
                    EXPECTED_ARGUMENTS_INCLUDING_BINARY);

                if (!startArguments.ContainsKey("port"))
                {
                    throw new System.ArgumentException("-port argument must be defined.");
                }

                UNetTransport transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UNetTransport;
                int listenPort = int.Parse(startArguments["port"]);

                if (!IsListenPortValid(listenPort))
                {
                    throw new
                        System.ArgumentException($"-port argument must be a valid port," +
                        $" got '{listenPort}'.");
                }

                transport.ServerListenPort = listenPort;

                NetworkManager.Singleton.StartServer();

                Debug.Log($"- Now listening to connections incoming from port '{listenPort}'.");
            }
        }

        private bool IsListenPortValid(int listenPort)
        {
            return (listenPort > 1024 && listenPort < 65535);
        }

        void Update()
        {
        
        }
    }
}
