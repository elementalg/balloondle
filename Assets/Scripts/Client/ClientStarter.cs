using Balloondle.Shared;
using Balloondle.Shared.Gameplay;
using Balloondle.Shared.Network.Game;
using MLAPI;
using UnityEngine;

namespace Balloondle.Client
{
    /// <summary>
    /// Provides the entry point to the game scene from the client's perspective.
    /// </summary>
    public class ClientStarter : MonoBehaviour
    {
        /// <summary>
        /// Prefab containing the PlayerController functionalities.
        /// </summary>
        [SerializeField]
        private GameObject playerControllerPrefab;

        /// <summary>
        /// Current instance being employed of the PlayerController.
        /// </summary>
        private GameObject playerController;

        /// <summary>
        /// Start the client by calling the NetworkManager.
        /// </summary>
        void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnConnectedToServer;
            NetworkManager.Singleton.StartClient();
        }

        /// <summary>
        /// Start listening to messages from the server once connected.
        /// </summary>
        /// <param name="clientId">This player's client Id.</param>
        private void OnConnectedToServer(ulong clientId)
        {
            if (NetworkManager.Singleton.IsClient)
            {
                // Look for the GameObject 'Messenger', which is the shared object
                // providing the communications between server and client.
                GameObject synchronizer = GameObject.Find("Messenger");
                synchronizer.GetComponent<NetworkRpcMessages>().OnSendMatchDetails += OnMatchDetails;
                synchronizer.GetComponent<NetworkRpcMessages>().OnPlayerSpawn += OnPlayerSpawn;
                synchronizer.GetComponent<NetworkRpcMessages>().OnSpawnPlayerBalloonAndWeapon += OnSpawnPlayerBalloonAndWeapon;
            }
        }

        /// <summary>
        /// Handle the incoming match details.
        /// </summary>
        /// <param name="map">Map which is going to be played.</param>
        private void OnMatchDetails(BaseMapFactory.Maps map)
        {
            Debug.Log($"Arrived match details: {map}");
        }

        /// <summary>
        /// Handle the spawn of the player's GameObject.
        /// </summary>
        /// <param name="targetClientId">Client Id for which client 
        /// this message has been meant to.</param>
        /// <param name="playerObjectId">Id of the player's objec</param>
        private void OnPlayerSpawn(ulong targetClientId, ulong playerObjectId)
        {
            if (NetworkManager.Singleton.LocalClientId != targetClientId)
            {
                Debug.Log("Skipping spawn of another client.");
                return;
            }

            // Look for the player objects.
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            // Detect the player object.
            foreach (GameObject playerObject in playerObjects)
            {
                if (playerObject.GetComponent<NetworkObject>().IsLocalPlayer) {
                    playerController = Instantiate(playerControllerPrefab);
                    playerController.GetComponent<PlayerController>().Player = playerObject;

                    break;
                }
            }
        }

        /// <summary>
        /// Detect when the player's balloon and weapon have been spawned.
        /// </summary>
        /// <param name="ownerId">Owner's network id.</param>
        /// <param name="objectIds">Network object ids of the balloon, thread cells and weapon.</param>
        private void OnSpawnPlayerBalloonAndWeapon(ulong ownerId, ulong[] objectIds)
        {
            // Ignore message, targeted at another client.
            if (NetworkManager.Singleton.LocalClientId != ownerId)
            {
                return;
            }

            // The first object id sent is alwyas the balloon.
            ulong balloonObjectId = objectIds[0];

            // The last object id sent is always the weapon.
            ulong weaponObjectId = objectIds[objectIds.Length - 1];

            MovableElement[] movableElements = GameObject.FindObjectsOfType<MovableElement>();

            PlayerController controller = playerController.GetComponent<PlayerController>();

            // Look for the balloon's and weapon's gameobjects.
            foreach (MovableElement element in movableElements)
            {
                ulong objectNetworkId = element.gameObject.GetComponent<NetworkObject>().NetworkObjectId;

                if (objectNetworkId == balloonObjectId)
                {
                    controller.PlayerBalloon = element.gameObject;
                    
                    if (controller.PlayerWeapon != null)
                    {
                        break;
                    }
                }
                else if (objectNetworkId == weaponObjectId)
                {
                    controller.PlayerWeapon = element.gameObject;
                        
                    if (controller.PlayerBalloon != null)
                    {
                        break;
                    }
                }
            }
        }
    }
}
