using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Client.UI.LobbyScene
{
    class ThreadCellsGenerator
    {
        /// <summary>
        /// GameObject instance containing the balloon's sprite and rigid body.
        /// </summary>
        private GameObject balloon;

        /// <summary>
        /// Point of the balloon where to tie the thread to.
        /// </summary>
        private Vector2 balloonAnchorPoint;

        /// <summary>
        /// GameObject instance containing the weapon's sprite and rigid body.
        /// </summary>
        private GameObject weapon;

        /// <summary>
        /// Point of the weapon where to tie the thread to.
        /// </summary>
        private Vector2 weaponAnchorPoint;

        /// <summary>
        /// Prefab used for generating thread cells.
        /// </summary>
        private GameObject threadCellPrefab;

        /// <summary>
        /// Size of a single thread cell.
        /// </summary>
        private float threadCellSize;

        /// <summary>
        /// List containing all the generated thread cells.
        /// </summary>
        private List<GameObject> threadCells;

        public ThreadCellsGenerator(GameObject balloon, Vector2 balloonAnchorPoint,
            GameObject weapon, Vector2 weaponAnchorPoint, GameObject threadCellPrefab, float threadCellSize)
        {
            this.balloon = balloon;
            this.balloonAnchorPoint = balloonAnchorPoint;

            this.weapon = weapon;
            this.weaponAnchorPoint = weaponAnchorPoint;

            this.threadCellPrefab = threadCellPrefab;
            this.threadCellSize = threadCellSize;
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
        public void GenerateThreadCells()
        {
            threadCells = new List<GameObject>();

            // Indicates the next Y position for the next generated cell.
            float nextY = GetBalloonAnchorPoint().y;

            float cellX = GetBalloonAnchorPoint().x;

            // Amount of cells needed to fill in the distance between the balloon
            // and the weapon.
            int cells = (int)
                Math.Ceiling(Vector3.Distance(GetBalloonAnchorPoint(),
                    GetWeaponAnchorPoint()) / threadCellSize);

            // Thread cell's anchor points.
            Vector2 threadCellAnchor = new Vector2(0f, -0.95f);
            Vector2 threadCellDestAnchor = new Vector2(0f, 0.95f);
            for (int i = 0; i < cells; i++)
            {
                Vector3 threadCellPosition = new Vector3(cellX, nextY, 0f);

                // Create instance of a thread cell.
                GameObject threadCell = GameObject.Instantiate(threadCellPrefab,
                    threadCellPosition, Quaternion.identity);
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
    }
}