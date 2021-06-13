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
            }
        }

        private void OnMatchDetails(BaseMapFactory.Maps map)
        {
            Debug.Log($"Arrived match details: {map}");
        }

        private void OnPlayerSpawn(ulong playerObjectId)
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject playerObject in playerObjects)
            {
                if (playerObject.GetComponent<NetworkObject>().IsLocalPlayer) {
                    GameObject controller = Instantiate(playerControllerPrefab);
                    controller.GetComponent<PlayerControllerFunctionality>().Player = playerObject;

                    break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
