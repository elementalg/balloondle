using Balloondle.Shared;
using Balloondle.Shared.Network.Game;
using MLAPI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Provides an accesible interface for spawning the balloons, weapons and thread cells.
    /// </summary>
    public class CharacterCreator : NetworkBehaviour
    {

        [SerializeField]
        private GameObject balloonPrefab;

        //
        // Point where the thread is tied to the balloon.
        //
        [SerializeField]
        private Vector2 balloonAnchorPoint;

        //
        // Instance of the player's weapon.
        //
        [SerializeField]
        private GameObject weaponPrefab;

        //
        // Point where the thread is tied to the weapon.
        //
        [SerializeField]
        private Vector2 weaponAnchorPoint;

        //
        // Instance of the base thread cell, used for creating all the
        // other thread cells.
        //
        [SerializeField]
        private GameObject baseThreadCell;

        //
        // Size of the edge of a thread cell.
        //
        [SerializeField]
        private float threadCellSize;

        private GameObject balloon;
        private GameObject weapon;

        //
        // List containing all the generated thread cells.
        //
        private List<GameObject> threadCells;

        /// <summary>
        /// Spawns the balloon, weapon and thread cells on a relative position from the 
        /// player object.
        /// </summary>
        /// <param name="playerObject"></param>
        public void SpawnBallonWithWeapon(GameObject playerObject)
        {
            threadCells = new List<GameObject>();

            ulong playerClientId = playerObject.GetComponent<NetworkObject>().OwnerClientId;

            SpawnBalloon(playerObject);
            balloon.GetComponent<MovableElement>().MovableByClientId = playerClientId;

            SpawnWeapon();
            weapon.GetComponent<MovableElement>().MovableByClientId = playerClientId;

            // Amount of cells needed to fill in the distance between the balloon
            // and the weapon.
            int cells = (int)
                Math.Ceiling(Vector3.Distance(GetBalloonAnchorPoint(),
                    GetWeaponAnchorPoint()) / threadCellSize);

            // Generate all thread cells by starting at the Y position of
            // the balloon's anchor point.
            GenerateThreadCells(GetBalloonAnchorPoint().y, cells);

            ulong[] objectIds = GetNetworkIdsForObjectsWithJoints();

            GameObject movementHandler = GameObject.FindGameObjectWithTag("MovementHandler");
            movementHandler
                .GetComponent<MovementHandlerFunctionality>()
                .OnSpawnedBalloonAndWeaponForPlayer(playerObject, balloon, threadCells, weapon);

            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer.GetComponent<NetworkRpcMessages>().OnSpawnPlayerBalloonAndWeaponClientRpc(playerClientId, objectIds);
        }

        /// <summary>
        /// Proceeds to spawn the balloon at the position of the specified gameObject.
        /// </summary>
        /// <param name="gameObject"></param>
        private void SpawnBalloon(GameObject gameObject)
        {
            balloon = GameObject.Instantiate(balloonPrefab, gameObject.transform.position, Quaternion.identity);
            balloon.GetComponent<NetworkObject>().Spawn();
        }

        /// <summary>
        /// Proceeds to spawn the weapon below the balloon.
        /// </summary>
        private void SpawnWeapon()
        {
            Vector3 weaponPosition = new Vector3(balloon.transform.position.x, balloon.transform.position.y - 4.4f, 0f);

            weapon = GameObject.Instantiate(weaponPrefab, weaponPosition, Quaternion.Euler(0f, 0f, -180f));
            weapon.GetComponent<NetworkObject>().Spawn();
            weapon.transform.position = weaponPosition;
        }

        //
        // Retrieves the world point where the thread is tied to
        // the player's balloon.
        //
        private Vector3 GetBalloonAnchorPoint()
        {
            // Transform the local position to the world one.
            Vector3 point = balloon.transform
                .TransformPoint(balloonAnchorPoint.x, balloonAnchorPoint.y, 0);

            return point;
        }

        //
        // Retrieves the world point where the thread is tied to
        // the player's weapon.
        //
        private Vector3 GetWeaponAnchorPoint()
        {
            // Transform the local position to the world one.
            Vector3 point = weapon.transform
                .TransformPoint(weaponAnchorPoint.x, weaponAnchorPoint.y, 0);

            return point;
        }

        //
        // Spawns thread cells on the screen.
        //
        private void GenerateThreadCells(float startY, int cells)
        {
            // Indicates the next Y position for the next generated cell.
            float nextY = startY;

            float cellX = GetBalloonAnchorPoint().x;

            // Thread cell's anchor points.
            Vector2 threadCellAnchor = new Vector2(0f, -0.95f);
            Vector2 threadCellDestAnchor = new Vector2(0f, 0.95f);
            for (int i = 0; i < cells; i++)
            {
                Vector3 threadCellPosition = new Vector3(cellX, nextY, 0f);

                // Create instance of a thread cell.
                GameObject threadCell = GameObject.Instantiate(baseThreadCell,
                    threadCellPosition, Quaternion.identity);
                threadCell.GetComponent<NetworkObject>().Spawn();
                threadCell.transform.position = threadCellPosition;

                // If at the start, link it with the balloon.
                if (i == 0)
                {
                    ConnectSourceWithJointsToDestination(balloon,
                        balloonAnchorPoint, threadCell,
                        threadCellDestAnchor);
                }
                else
                {
                    // If at the end, link it with the weapon.
                    if (i == cells - 1)
                    {
                        ConnectSourceWithJointsToDestination(threadCell,
                            threadCellAnchor, weapon,
                            weaponAnchorPoint);
                    }

                    // Link with the previous thread cell.
                    ConnectSourceWithJointsToDestination(threadCells[i - 1],
                        threadCellAnchor, threadCell,
                        threadCellDestAnchor);
                }

                // Add the thread cell to the list.
                threadCells.Add(threadCell);

                // Update the Y position for the next thread cell.
                nextY -= threadCellSize;
            }
        }

        //
        // Connects the source's joint with the destination's rigid body.
        //
        private void ConnectSourceWithJointsToDestination(GameObject source,
            Vector2 sourceAnchor, GameObject destination,
            Vector2 destinationAnchor)
        {
            HingeJoint2D hingeJoint2D = source.GetComponent<HingeJoint2D>();

            Rigidbody2D body = destination.GetComponent<Rigidbody2D>();

            // Update anchors and connected body.
            hingeJoint2D.connectedBody = body;
            hingeJoint2D.anchor = sourceAnchor;
            hingeJoint2D.connectedAnchor = destinationAnchor;
        }

        /// <summary>
        /// Retrieves the network object's ids.
        /// </summary>
        /// <returns>Network object ids for the balloon and thread cells.</returns>
        private ulong[] GetNetworkIdsForObjectsWithJoints()
        {
            ulong[] objectIds = new ulong[threadCells.Count + 2];

            objectIds[0] = GetNetworkIdOfObject(balloon);

            for (int i = 0; i < threadCells.Count; i++)
            {
                objectIds[i + 1] = GetNetworkIdOfObject(threadCells[i]);
            }

            objectIds[objectIds.Length - 1] = GetNetworkIdOfObject(weapon);

            Debug.Log($"{objectIds[0]} && {objectIds[1]} && {objectIds[objectIds.Length - 1]}");

            return objectIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject">GameObject containing a NetworkObject component.</param>
        /// <returns>Get the network gameobject id.</returns>
        private ulong GetNetworkIdOfObject(GameObject gameObject)
        {
            return gameObject.GetComponent<NetworkObject>().NetworkObjectId;
        }
    }
}
