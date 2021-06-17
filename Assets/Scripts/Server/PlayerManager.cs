using Balloondle.Shared;
using Balloondle.Shared.Network.Game;
using MLAPI;
using MLAPI.Configuration;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Provides a way to spawn players by their network client id.
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        /// <summary>
        /// Prefab containing the player.
        /// </summary>
        private NetworkPrefab playerPrefab;

        /// <summary>
        /// Dictionary containing the gameobjects of the server players.
        /// </summary>
        private Dictionary<ulong, GameObject> serverPlayers;

        /// <summary>
        /// Retrieve the network player's prefab.
        /// </summary>
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

        /// <summary>
        /// Spawn all the players.
        /// </summary>
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

        /// <summary>
        /// Spawn a player by their client id.
        /// </summary>
        /// <param name="clientId"></param>
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

        /// <summary>
        /// Retrieves a random position from the current map.
        /// </summary>
        /// <returns>Random position from the map</returns>
        private Vector3 GetRandomPositionFromMap()
        {
            GameObject map = GameObject.FindGameObjectWithTag("Map");

            return map.GetComponent<MapSpawnLocations>().GetRandomSpawnLocation();
        }
    }
}
