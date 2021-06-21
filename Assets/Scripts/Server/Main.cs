using Balloondle.Shared;
using MLAPI;
using MLAPI.Transports.UNET;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Server
{
    /// <summary>
    /// Class to handle the execution of the server from a command line interface.
    /// </summary>
    public class Main : MonoBehaviour
    {
        /// <summary>
        /// Amount of arguments expected to be appended to the execution
        /// of the gameserver's binary file.
        /// </summary>
        private const int EXPECTED_ARGUMENTS_INCLUDING_BINARY = 11;

        /// <summary>
        /// Handle the start by handling the scene loaded event.
        /// </summary>
        void Start()
        {
            Debug.Log("- Starting.");

            Debug.Log("- Preparing to listen to incoming connections.");

            // Retrieve the gameserver's details from the command line.
            CommandLineArgumentsParser parser = new CommandLineArgumentsParser();
            Dictionary<string, string> startArguments = parser
                .GetExpectedCommandLineArguments(System.Environment.GetCommandLineArgs(),
                EXPECTED_ARGUMENTS_INCLUDING_BINARY);

            ExceptionIfMissingArguments(startArguments);

            string gamemode = startArguments["gamemode"];
            string map = startArguments["map"];
            long code = long.Parse(startArguments["code"]);
            string lobbyBaseUrl = startArguments["lobby"];

            int listenPort = int.Parse(startArguments["port"]);

            if (!IsListenPortValid(listenPort))
            {
                throw new
                    System.ArgumentException($"-port argument must be a valid port," +
                    $" got '{listenPort}'.");
            }

            GetComponent<ServerStarter>().StartServer(map, gamemode, code, lobbyBaseUrl, listenPort);

            Debug.Log($"- Now listening to connections incoming from port '{listenPort}'.");
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

            if (!startArguments.ContainsKey("lobby"))
            {
                throw new System.ArgumentException("-lobby argument must be defined.");
            }

            if (!startArguments.ContainsKey("code"))
            {
                throw new System.ArgumentException("-code argument must be defined.");
            }
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