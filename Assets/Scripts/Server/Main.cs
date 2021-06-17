using Balloondle.Shared;
using MLAPI;
using MLAPI.Transports.UNET;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Server
{
    /// <summary>
    /// DEPRECATED - To be edited in order to adapt to new architecture.
    /// Class to handle the execution of the server from a command line interface.
    /// </summary>
    public class Main : MonoBehaviour
    {
        /// <summary>
        /// Amount of arguments expected to be appended to the execution
        /// of the gameserver's binary file.
        /// </summary>
        private const int EXPECTED_ARGUMENTS_INCLUDING_BINARY = 9;

        /// <summary>
        /// Handle the start by handling the scene loaded event.
        /// </summary>
        void Start()
        {
            Debug.Log("- Starting.");

            SceneManager.sceneLoaded += OnSharedSceneHasBeenLoaded;
        }

        /// <summary>
        /// Detect when the shared scene has been loaded.
        /// </summary>
        /// <param name="loadedScene"></param>
        /// <param name="mode"></param>
        void OnSharedSceneHasBeenLoaded(Scene loadedScene, LoadSceneMode mode)
        {
            if (loadedScene.name.Equals(Scenes.GameSharedScene.ToString()))
            {
                Debug.Log("- Preparing to listen to incoming connections.");

                // Retrieve the gameserver's details from the command line.
                CommandLineArgumentsParser parser = new CommandLineArgumentsParser();
                Dictionary<string, string> startArguments = parser
                    .GetExpectedCommandLineArguments(System.Environment.GetCommandLineArgs(),
                    EXPECTED_ARGUMENTS_INCLUDING_BINARY);

                ExceptionIfMissingArguments(startArguments);

                string gamemode = startArguments["gamemode"];
                string map = startArguments["map"];

                int listenPort = int.Parse(startArguments["port"]);
                
                StartServerOnPort(listenPort);

                Debug.Log($"- Now listening to connections incoming from port '{listenPort}'.");
            }
        }

        /// <summary>
        /// Throw an exception if certain parameter is missing.
        /// </summary>
        /// <param name="startArguments">Dictionary containing the start parameters.</param>
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

        /// <summary>
        /// Start the server on the specified port.
        /// </summary>
        /// <param name="listenPort"></param>
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

            NetworkManager.Singleton.StartServer();
        }

        /// <summary>
        /// Check whether or not the port is valid.
        /// </summary>
        /// <param name="listenPort">Port to be listened.</param>
        /// <returns></returns>
        private bool IsListenPortValid(int listenPort)
        {
            return (listenPort > 1024 && listenPort < 65535);
        }
    }
}