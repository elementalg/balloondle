using Balloondle.Shared;
using Balloondle.Shared.Gameplay;
using Balloondle.Shared.Network.Game;
using MLAPI;
using UnityEngine;

namespace Balloondle.Client
{
    public class ClientStarter : MonoBehaviour
    {

        [SerializeField]
        private GameObject playerControllerPrefab;

        private GameObject playerController;

        // Start is called before the first frame update
        void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnConnectedToServer;
            NetworkManager.Singleton.StartClient();
        }

        private void OnConnectedToServer(ulong clientId)
        {
            if (NetworkManager.Singleton.IsClient)
            {
                GameObject synchronizer = GameObject.Find("Messenger");
                synchronizer.GetComponent<NetworkRpcMessages>().OnSendMatchDetails += OnMatchDetails;
                synchronizer.GetComponent<NetworkRpcMessages>().OnPlayerSpawn += OnPlayerSpawn;
                synchronizer.GetComponent<NetworkRpcMessages>().OnSpawnPlayerBalloonAndWeapon += OnSpawnPlayerBalloonAndWeapon;
            }
        }

        private void OnMatchDetails(BaseMapFactory.Maps map)
        {
            Debug.Log($"Arrived match details: {map}");
        }

        private void OnPlayerSpawn(ulong targetClientId, ulong playerObjectId)
        {
            if (NetworkManager.Singleton.LocalClientId != targetClientId)
            {
                Debug.Log("Skipping spawn of another client.");
                return;
            }

            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject playerObject in playerObjects)
            {
                if (playerObject.GetComponent<NetworkObject>().IsLocalPlayer) {
                    playerController = Instantiate(playerControllerPrefab);
                    playerController.GetComponent<PlayerControllerFunctionality>().Player = playerObject;

                    break;
                }
            }
        }

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

            PlayerControllerFunctionality controller = playerController.GetComponent<PlayerControllerFunctionality>();

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

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
