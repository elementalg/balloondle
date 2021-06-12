using Balloondle.Server.Gameplay;
using Balloondle.Server.Network;
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

        private Match match;

        [SerializeField]
        private MapFactory mapFactory;

        private INetworkEventHandler networkEventHandler;

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

                networkEventHandler = new NetworkEventHandlerImpl(match, startArguments["password"]);

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
            NetworkManager.Singleton.ConnectionApprovalCallback += networkEventHandler
                .OnConnectionApprovalRequest;

            NetworkManager.Singleton.OnClientConnectedCallback += networkEventHandler
                .OnClientConnected;

            NetworkManager.Singleton.OnClientDisconnectCallback += networkEventHandler
                .OnClientDisconnected;
        }

        private void LoadMatch(string gamemodeName, string mapName)
        {
            GamemodeFactory.Gamemodes gamemode;

            bool isGamemodeNameValid = System
                .Enum
                .TryParse<GamemodeFactory.Gamemodes>(gamemodeName, out gamemode);

            if (!isGamemodeNameValid)
            {
                throw new System
                    .ArgumentException("Gamemode name is not a valid one.", nameof(gamemodeName));
            }


            bool isMapNameValid = System.Enum.TryParse<MapFactory.Maps>(mapName, out MapFactory.Maps map);

            if (!isMapNameValid)
            {
                throw new System.ArgumentException("Map name is not a valid one.", nameof(mapName));
            }

            GamemodeFactory gamemodeFactory = new GamemodeFactory();

            match = new Match(gamemodeFactory.BuildGamemode(gamemode), mapFactory.BuildMap(map));

            match.Start();
        }

        void Update()
        {
      
        }
    }
}