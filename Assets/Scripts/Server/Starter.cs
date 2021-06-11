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
    public class Starter : MonoBehaviour
    {
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

                Dictionary<string, string> startArguments = GetExpectedCommandLineArguments();

                if (!startArguments.ContainsKey("port"))
                {
                    throw new System.ArgumentException("-port argument must be defined.");
                }
                UNetTransport transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UNetTransport;
                int listenPort = int.Parse(startArguments["port"]);

                if (!IsListenPortValid(listenPort))
                {
                    throw new
                        System.ArgumentException($"-port argument must be a valid port, got '{listenPort}'.");
                }

                transport.ServerListenPort = listenPort;

                NetworkManager.Singleton.StartServer();

                Debug.Log($"- Now listening to connections incoming from port '{listenPort}'.");
            }
        }
        
        private Dictionary<string, string> GetExpectedCommandLineArguments()
        {
            string[] commandLineArguments = System.Environment.GetCommandLineArgs();
            Dictionary<string, string> arguments = new Dictionary<string, string>();

            if (commandLineArguments.Length < 2)
            {
                throw new 
                    System.InvalidOperationException("Failed to acquire required arguments.");
            }

            for (int i = 0; i < commandLineArguments.Length; i++)
            {
                string argument = commandLineArguments[i].ToLower();

                if (IsArgumentAParameterIndicator(argument))
                {
                    if ( i + 1 < commandLineArguments.Length)
                    {
                        string nextArgument = commandLineArguments[i + 1];

                        if (IsArgumentAParameterIndicator(nextArgument))
                        {
                            arguments.Add(argument, "");
                        } 
                        else
                        {
                            arguments.Add(argument, nextArgument);
                        }
                    }
                }
            }

            return arguments;
        }

        private bool IsArgumentAParameterIndicator(string argument)
        {
            return argument.StartsWith("-");
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
