using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Start point for the server.
    /// </summary>
    public class ServerStarter : MonoBehaviour
    {
        /// <summary>
        /// Name of the map to be played.
        /// </summary>
        [SerializeField]
        private string map = "dev";

        /// <summary>
        /// Name of the gamemode to be played.
        /// </summary>
        [SerializeField]
        private string gamemode = "dev";

        /// <summary>
        /// Maximum amount of allowed players.
        /// </summary>
        [SerializeField]
        private uint maxPlayers = 6;

        /// <summary>
        /// Prefab of the match object.
        /// </summary>
        [SerializeField]
        private GameObject matchObjectPrefab;

        /// <summary>
        /// Prefab containing the object in charge of communicating the state of the match to the lobby.
        /// </summary>
        [SerializeField]
        private GameObject lobbyMatchCommunicatorPrefab;

        /// <summary>
        /// Prefab of the movement handler.
        /// </summary>
        [SerializeField]
        private GameObject movementHandlerPrefab;

        /// <summary>
        /// Start listening to the events, starting also the match.
        /// </summary>
        public void StartServer(string map, string gamemode, long code, string lobbyBaseUrl, int listenPort)
        {
            this.map = map;
            this.gamemode = gamemode;

            GameObject lobbyMatchCommunicator = GameObject.Instantiate(lobbyMatchCommunicatorPrefab);
            LobbyMatchCommunicator communicator = lobbyMatchCommunicator.GetComponent<LobbyMatchCommunicator>();
            communicator.InitializeCommunicator(map, gamemode, code, lobbyBaseUrl);

            UNetTransport transport = NetworkManager
                    .Singleton
                    .NetworkConfig
                    .NetworkTransport as UNetTransport;

            transport.ServerListenPort = listenPort;

            GameObject movementHandler = GameObject.Instantiate(movementHandlerPrefab);

            GameObject match = GameObject.Instantiate(matchObjectPrefab);
            MatchFunctionality matchFunctionality = match.GetComponent<MatchFunctionality>();
            matchFunctionality.Gamemode = gamemode;
            matchFunctionality.Map = map;
            matchFunctionality.MaxPlayers = maxPlayers;

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.StartServer();

            communicator.UpdateServerMatchStateToRunning();
        }

        private void OnClientConnect(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                GameObject gamemodeObject = GameObject.FindGameObjectWithTag("Gamemode");
                gamemodeObject.GetComponent<BaseGamemode>().OnClientJoin(clientId);
            }
        }

        private void OnClientDisconnect(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                GameObject gamemodeObject = GameObject.FindGameObjectWithTag("Gamemode");
                gamemodeObject.GetComponent<BaseGamemode>().OnClientDisconnect(clientId);
            }
        }
    }
}
