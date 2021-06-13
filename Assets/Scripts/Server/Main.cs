using Balloondle.Shared;
using MLAPI;
using MLAPI.Transports.UNET;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Server
{
    /// <summary>
    /// Class to be appended with a root object for the scene containing the game server's logic, so
    /// the server starts listening for incoming connections as specified by the incoming command line
    /// arguments.
    /// </summary>
    public class Main : MonoBehaviour
    {
        private const int EXPECTED_ARGUMENTS_INCLUDING_BINARY = 9;

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

                ExceptionIfMissingArguments(startArguments);

                string gamemode = startArguments["gamemode"];
                string map = startArguments["map"];

                LoadMatch(gamemode, map);


                int listenPort = int.Parse(startArguments["port"]);
                
                StartServerOnPort(listenPort);

                Debug.Log($"- Now listening to connections incoming from port '{listenPort}'.");
            }
        }

        private void ExceptionIfMissingArguments(Dictionary<string, string> startArguments)
        {
            if (!startArguments.ContainsKey("port"))
            {
                throw new System.ArgumentException("-port argument must be defined.");
            }

            if (!startArguments.ContainsKey("gamemode"))
            {
                throw new System.ArgumentException("-gamemode argument must be defined.");
            }

            if (!startArguments.ContainsKey("map"))
            {
                throw new System.ArgumentException("-map argument must be defined.");
            }

            if (!startArguments.ContainsKey("password"))
            {
                throw new System.ArgumentException("-password argument must be defined.");
            }
        }

        private void StartServerOnPort(int listenPort)
        {
            UNetTransport transport = NetworkManager
                    .Singleton
                    .NetworkConfig
                    .NetworkTransport as UNetTransport;

            if (!IsListenPortValid(listenPort))
            {
                throw new
                    System.ArgumentException($"-port argument must be a valid port," +
                    $" got '{listenPort}'.");
            }

            transport.ServerListenPort = listenPort;

            LinkNetworkEventHandlerWithNetworkManager();

            NetworkManager.Singleton.StartServer();
        }

        private bool IsListenPortValid(int listenPort)
        {
            return (listenPort > 1024 && listenPort < 65535);
        }

        private void LinkNetworkEventHandlerWithNetworkManager()
        {
           
        }

        private void LoadMatch(string gamemodeName, string mapName)
        {
            
        }

        void Update()
        {
      
        }
    }
}