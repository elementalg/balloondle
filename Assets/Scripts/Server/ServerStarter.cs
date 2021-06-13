using Balloondle.Shared;
using Balloondle.Shared.Gameplay;
using Balloondle.Shared.Network.Game;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Balloondle.Server
{
    public class ServerStarter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.StartServer();

            GetComponent<MapLoader>().LoadMap(BaseMapFactory.Maps.DEVELOPMENT);
        }

        private void OnClientConnect(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"Server: ClientId Connected {clientId}");

                GameObject synchronizer = GameObject.Find("Messenger");
                synchronizer.GetComponent<NetworkRpcMessages>().OnSendMatchDetailsToClientRpc(BaseMapFactory.Maps.DEVELOPMENT);

                var prefabs = NetworkManager.Singleton.NetworkConfig.NetworkPrefabs;
               
                foreach (var prefab in prefabs)
                {
                    if (prefab.PlayerPrefab)
                    {
                        GameObject playerObject = GameObject.Instantiate(prefab.Prefab);
                        playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
                        playerObject.GetComponent<PlayerData>().playerName = new NetworkVariable<string>($"Player {clientId}");
                        playerObject.GetComponent<PlayerData>().clientId = new NetworkVariable<ulong>(clientId);

                        synchronizer.GetComponent<NetworkRpcMessages>().OnPlayerSpawnClientRpc(playerObject.GetComponent<NetworkObject>().NetworkObjectId);

                        break;
                    }
                }
            }
        }

        private void OnClientDisconnect(ulong clientId)
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
