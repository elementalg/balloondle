using Balloondle.Shared.Network.Game;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Balloondle.Client
{
    public class PlayerControllerFunctionality : MonoBehaviour
    {
        [SerializeField]
        private Vector2 balloonMovementScale = new Vector2(1f, 1f);

        [SerializeField]
        private Vector2 weaponMovementScale = new Vector2(1f, 1f);

        public GameObject Player { get; set; }
        public GameObject PlayerBalloon { get; set; }
        public GameObject PlayerWeapon { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            
        }
        
        public void MoveBalloon(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();

            movement.Scale(balloonMovementScale);

            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer
                .GetComponent<NetworkRpcMessages>()
                .OnPlayerMoveBalloonServerRpc(NetworkManager.Singleton.LocalClientId, movement);
        }

        public void MoveWeapon(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();

            movement.Scale(weaponMovementScale);

            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer
                .GetComponent<NetworkRpcMessages>()
                .OnPlayerMoveWeaponServerRpc(NetworkManager.Singleton.LocalClientId, movement);
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerBalloon != null)
            {
                Vector3 position = new Vector3(PlayerBalloon.transform.position.x, PlayerBalloon.transform.position.y, -10f);

                GetComponent<Camera>().transform.SetPositionAndRotation(position, Quaternion.identity);
            }
        }
    }
}
