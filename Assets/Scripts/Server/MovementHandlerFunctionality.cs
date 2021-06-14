using Balloondle.Shared.Network.Game;
using MLAPI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Server
{
    public class MovementHandlerFunctionality : MonoBehaviour
    {
        private Dictionary<ulong, Tuple<GameObject, List<GameObject>, GameObject>> playerObjects;

        // Start is called before the first frame update
        void Start()
        {
            playerObjects = new Dictionary<ulong, Tuple<GameObject, List<GameObject>, GameObject>>();

            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer.GetComponent<NetworkRpcMessages>().OnPlayerMoveBalloon += OnPlayerMoveBalloon;
            synchronizer.GetComponent<NetworkRpcMessages>().OnPlayerMoveWeapon += OnPlayerMoveWeapon;
        }

        public void OnPlayerMoveBalloon(ulong playerClientId, Vector2 movement)
        {
            if (playerObjects.ContainsKey(playerClientId))
            {
                GameObject balloon = playerObjects[playerClientId].Item1;
                Rigidbody2D balloonBody = balloon.GetComponent<Rigidbody2D>();

                balloonBody.AddForce(movement);
            }
        }

        public void OnPlayerMoveWeapon(ulong playerClientId, Vector2 movement)
        {
            if (playerObjects.ContainsKey(playerClientId))
            {
                GameObject weapon = playerObjects[playerClientId].Item3;
                Rigidbody2D weaponBody = weapon.GetComponent<Rigidbody2D>();

                weaponBody.AddForce(movement);
            }
        }

        public void OnSpawnedBalloonAndWeaponForPlayer(GameObject player, GameObject balloon,
            List<GameObject> threadCells, GameObject weapon)
        {

            ulong playerClientId = player.GetComponent<NetworkObject>().NetworkObjectId;

            if (playerObjects.ContainsKey(playerClientId))
            {
                DestroyInstancesOfPlayerObjects(playerClientId);
            }

            Tuple<GameObject, List<GameObject>, GameObject> objects = 
                new Tuple<GameObject, List<GameObject>, GameObject>(balloon, threadCells, weapon);

            playerObjects[playerClientId] = objects;
        }

        // TODO: Handle OnDestroy

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

        // Update is called once per frame
        void Update()
        {

        }
    }
}
