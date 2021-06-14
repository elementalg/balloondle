using Balloondle.Shared;
using Balloondle.Shared.Network.Game;
using MLAPI;
using MLAPI.Configuration;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Server
{
    public class PlayerManager : MonoBehaviour
    {
        private NetworkPrefab playerPrefab;

        private Dictionary<ulong, GameObject> serverPlayers;

        private void Start()
        {
            serverPlayers = new Dictionary<ulong, GameObject>();

            var prefabs = NetworkManager.Singleton.NetworkConfig.NetworkPrefabs;

            foreach (var prefab in prefabs)
            {
                if (prefab.PlayerPrefab)
                {
                    playerPrefab = prefab;

                    break;
                }
            }
        }

        public void SpawnPlayers()
        {
            var playerList = NetworkManager.Singleton.ConnectedClientsList;

            foreach (var player in playerList)
            {
                Debug.Log("Player");
                if (player.ClientId != NetworkManager.Singleton.ServerClientId)
                {
                    Debug.Log("ClientId");

                    SpawnPlayer(player.ClientId);
                }
            }
        }

        public void SpawnPlayer(ulong clientId)
        {
            Debug.Log($"Spawning player: {clientId}");

            GameObject playerObject = GameObject
                .Instantiate(playerPrefab.Prefab, GetRandomPositionFromMap(), Quaternion.identity);
            playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

            ulong playerObjectId = playerObject.GetComponent<NetworkObject>().NetworkObjectId;
            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer.GetComponent<NetworkRpcMessages>().OnPlayerSpawnClientRpc(clientId, playerObjectId);

            GetComponent<CharacterCreator>().SpawnBallonWithWeapon(playerObject);
        }

        private Vector3 GetRandomPositionFromMap()
        {
            GameObject map = GameObject.FindGameObjectWithTag("Map");

            return map.GetComponent<MapSpawnLocations>().GetRandomSpawnLocation();
        }
    }
}
