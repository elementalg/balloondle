using Balloondle.Shared.Gameplay;
using Balloondle.Shared.Network.Game;
using MLAPI;
using UnityEngine;

namespace Balloondle.Server
{
    public class ServerStarter : MonoBehaviour
    {
        [SerializeField]
        private string map = "CubeLand";

        [SerializeField]
        private string gamemode = "FreeForAll";

        [SerializeField]
        private uint maxPlayers = 6;

        [SerializeField]
        private GameObject matchObjectPrefab;

        // Start is called before the first frame update
        void Start()
        {
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

        private void OnClientAwaitsForApproval(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
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

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
