using Balloondle.Shared.Network.Game;
using MLAPI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Provides the basic functionality for the movement handling.
    /// </summary>
    public class MovementHandlerFunctionality : MonoBehaviour
    {
        /// <summary>
        /// Dictionary containing the balloon, weapon and thread cells of each player.
        /// </summary>
        private Dictionary<ulong, Tuple<GameObject, List<GameObject>, GameObject>> playerObjects;

        /// <summary>
        /// Listen to the movement related events and proceed to initialize the dictionary.
        /// </summary>
        void Start()
        {
            playerObjects = new Dictionary<ulong, Tuple<GameObject, List<GameObject>, GameObject>>();

            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer.GetComponent<NetworkRpcMessages>().OnPlayerMoveBalloon += OnPlayerMoveBalloon;
            synchronizer.GetComponent<NetworkRpcMessages>().OnPlayerMoveWeapon += OnPlayerMoveWeapon;

            NetworkManager.Singleton.OnClientDisconnectCallback += DestroyInstancesOfPlayerObjects;
        }

        /// <summary>
        /// Detect the message from the player, and proceed to move the balloon.
        /// </summary>
        /// <param name="playerClientId">Client Id of the player which has requested a movement.</param>
        /// <param name="movement">Force to be applied to the balloon.</param>
        public void OnPlayerMoveBalloon(ulong playerClientId, Vector2 movement)
        {
            Debug.Log("Move Balloon");
            if (playerObjects.ContainsKey(playerClientId))
            {
                Debug.Log("Detected Balloon");
                GameObject balloon = playerObjects[playerClientId].Item1;
                Rigidbody2D balloonBody = balloon.GetComponent<Rigidbody2D>();

                balloonBody.AddForce(movement);
            }
        }

        /// <summary>
        /// Detect the message from the player, and proceed to move the weapon.
        /// </summary>
        /// <param name="playerClientId">Client Id of the player requesting the movement.</param>
        /// <param name="movement">Force to be applied to the weapon.</param>
        public void OnPlayerMoveWeapon(ulong playerClientId, Vector2 movement)
        {
            Debug.Log("Move Weapon");
            if (playerObjects.ContainsKey(playerClientId))
            {
                Debug.Log("Detected Weapon");

                GameObject weapon = playerObjects[playerClientId].Item3;
                Rigidbody2D weaponBody = weapon.GetComponent<Rigidbody2D>();

                weaponBody.AddForce(movement);
            }
        }

        /// <summary>
        /// Proceed to store the balloon, threadCells and weapon of a player.
        /// Destroying the previous gameobjects, in case there are any.
        /// </summary>
        /// <param name="player">Player whose balloon, thread cells and weapon have been spawned.</param>
        /// <param name="balloon">Balloon of the player.</param>
        /// <param name="threadCells">Thread cells of the player.</param>
        /// <param name="weapon">Weapon of the player.</param>
        public void OnSpawnedBalloonAndWeaponForPlayer(GameObject player, GameObject balloon,
            List<GameObject> threadCells, GameObject weapon)
        {

            ulong playerClientId = player.GetComponent<NetworkObject>().OwnerClientId;

            if (playerObjects.ContainsKey(playerClientId))
            {
                DestroyInstancesOfPlayerObjects(playerClientId);
            }

            Tuple<GameObject, List<GameObject>, GameObject> objects = 
                new Tuple<GameObject, List<GameObject>, GameObject>(balloon, threadCells, weapon);

            playerObjects[playerClientId] = objects;
        }

        // TODO: Handle OnDestroy

        /// <summary>
        /// Destroy the instances of the player objects.
        /// </summary>
        /// <param name="playerClientId">Client Id of the player whose objects are going
        /// to be destroyed.</param>
        public void DestroyInstancesOfPlayerObjects(ulong playerClientId)
        {
            if (playerObjects.ContainsKey(playerClientId))
            {
                Tuple<GameObject, List<GameObject>, GameObject> objects = playerObjects[playerClientId];

                // Destroy the balloon.
                Destroy(objects.Item1);

                // Destroy the thread cells.
                foreach (GameObject threadCell in objects.Item2)
                {
                    Destroy(threadCell);
                }

                // Destroy the weapon.
                Destroy(objects.Item3);

                playerObjects.Remove(playerClientId);
            }
        }
    }
}
