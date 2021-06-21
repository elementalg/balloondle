using Balloondle.Shared.Network.Game;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Balloondle.Client
{
    /// <summary>
    /// Provides the base client side functionality for the balloon's and weapon's movement.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Scales the input for the balloon movement by the indicated scale.
        /// </summary>
        [SerializeField]
        private Vector2 balloonMovementScale = new Vector2(1f, 1f);

        /// <summary>
        /// Scales the input for the weapon movement by the indicated scale.
        /// </summary>
        [SerializeField]
        private Vector2 weaponMovementScale = new Vector2(1f, 1f);

        /// <summary>
        /// Prefab containing the object which has got the on screen joysticks.
        /// </summary>
        [SerializeField]
        private GameObject joysticksCanvasPrefab;

        /// <summary>
        /// Prefab containing the required GameObject of Input Event System.
        /// </summary>
        [SerializeField]
        private GameObject inputEventSystemPrefab;

        /// <summary>
        /// GameObject containing LocalPlayer's Network Object.
        /// </summary>
        public GameObject Player { get; set; }

        /// <summary>
        /// GameObject which is the balloon of the LocalPlayer.
        /// </summary>
        public GameObject PlayerBalloon { get; set; }

        /// <summary>
        /// GameObject which is the weapon of the LocalPlayer.
        /// </summary>
        public GameObject PlayerWeapon { get; set; }

        [SerializeField]
        private float balloonMovementSampleDurationInSeconds = 0.1f;

        [SerializeField]
        private float weaponMovementSampleDurationInSeconds = 0.1f;

        private InputMaximizer balloonMovementMaximizer;
        private InputMaximizer weaponMovementMaximizer;

        private NetworkRpcMessages messenger;

        /// <summary>
        /// Create the instances of the prefabs.
        /// </summary>
        void Start()
        {
            GameObject.Instantiate(inputEventSystemPrefab);
            GameObject.Instantiate(joysticksCanvasPrefab);

            balloonMovementMaximizer = new InputMaximizer(balloonMovementSampleDurationInSeconds);
            weaponMovementMaximizer = new InputMaximizer(weaponMovementSampleDurationInSeconds);

            GameObject synchronizer = GameObject.Find("Messenger");
            messenger = synchronizer.GetComponent<NetworkRpcMessages>();
        }
        
        /// <summary>
        /// Detect the input and send the details to the server, in order to move the balloon.
        /// </summary>
        /// <param name="context"></param>
        public void MoveBalloon(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();

            movement.Scale(balloonMovementScale);

            balloonMovementMaximizer.OnInput(movement);
        }

        /// <summary>
        /// Detect the input and send the details to the server, in order to move the weapon.
        /// </summary>
        /// <param name="context"></param>
        public void MoveWeapon(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();

            movement.Scale(weaponMovementScale);

            weaponMovementMaximizer.OnInput(movement);
        }

        /// <summary>
        /// Fix the camera on the player.
        /// </summary>
        void Update()
        {
            if (PlayerBalloon != null)
            {
                Vector2? balloonMaximizedMovement = balloonMovementMaximizer.OnUpdate();

                if (balloonMaximizedMovement.HasValue)
                {
                    Debug.Log($"Sending balloon: {balloonMaximizedMovement.Value}");
                    messenger.OnPlayerMoveBalloonServerRpc(NetworkManager.Singleton.LocalClientId,
                        balloonMaximizedMovement.Value);
                }

                Vector2? weaponMaximizedMovement = weaponMovementMaximizer.OnUpdate();

                if (weaponMaximizedMovement.HasValue)
                {
                    Debug.Log($"Sending weapon: {weaponMaximizedMovement.Value}");
                    messenger.OnPlayerMoveWeaponServerRpc(NetworkManager.Singleton.LocalClientId,
                        weaponMaximizedMovement.Value);
                }

                Vector3 position = new Vector3(PlayerBalloon.transform.position.x,
                    PlayerBalloon.transform.position.y, -10f);

                GetComponent<Camera>().transform.SetPositionAndRotation(position, Quaternion.identity);
            }
        }
    }
}
