using MLAPI;
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
        private string map = "CubeLand";

        /// <summary>
        /// Name of the gamemode to be played.
        /// </summary>
        [SerializeField]
        private string gamemode = "FreeForAll";

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
        /// Prefab of the movement handler.
        /// </summary>
        [SerializeField]
        private GameObject movementHandlerPrefab;

        /// <summary>
        /// Start listening to the events, starting also the match.
        /// </summary>
        void Start()
        {
            GameObject movementHandler = GameObject.Instantiate(movementHandlerPrefab);

            GameObject match = GameObject.Instantiate(matchObjectPrefab);
            MatchFunctionality matchFunctionality = match.GetComponent<MatchFunctionality>();
            matchFunctionality.Gamemode = gamemode;
            matchFunctionality.Map = map;
            matchFunctionality.MaxPlayers = maxPlayers;

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.ConnectionApprovalCallback += OnClientAwaitsForApproval;
            NetworkManager.Singleton.StartServer();
        }

        /// <summary>
        /// Approves the connection only if the amount of maximum allowed players has not been passed.
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="clientId"></param>
        /// <param name="callback"></param>
        private void OnClientAwaitsForApproval(byte[] connectionData,
            ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            if (NetworkManager.Singleton.ConnectedClientsList.Count > maxPlayers)
            {
                callback(false, null, false, null, null);
                return;
            }

            callback(false, null, true, null, null);
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
