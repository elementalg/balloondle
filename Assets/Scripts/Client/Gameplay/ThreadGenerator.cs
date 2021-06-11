/*
 * ThreadBehaviour.cs
 *
 * Generator of threads.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

//
// Creates a thread composed by many cells which occupy the total distance
// between the player's balloon and his weapon.
//
public class ThreadGenerator : MonoBehaviour
{
    //
    // Instance of the player's balloon.
    //
    [SerializeField]
    private GameObject balloon;

    //
    // Point where the thread is tied to the balloon.
    //
    [SerializeField]
    private Vector2 balloonAnchorPoint;

    //
    // Instance of the player's weapon.
    //
    [SerializeField]
    private GameObject weapon;

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

    //
    // List containing all the generated thread cells.
    //
    private List<GameObject> threadCells = new List<GameObject>();

    //
    // Start is called before the first frame update.
    //
    void Start()
    {
        // Amount of cells needed to fill in the distance between the balloon
        // and the weapon.
        int cells = (int)
            Math.Ceiling(Vector3.Distance(GetBalloonAnchorPoint(),
                GetWeaponAnchorPoint()) / threadCellSize);

        // Generate all thread cells by starting at the Y position of
        // the balloon's anchor point.
        GenerateThreadCells(GetBalloonAnchorPoint().y, cells);
    }

    //
    // Retrieves the world point where the thread is tied to
    // the player's balloon.
    //
    private Vector3 GetBalloonAnchorPoint()
    {
        // Transform the local position to the world one.
        Vector3 point = balloon.transform
            .TransformPoint(balloonAnchorPoint.x, balloonAnchorPoint.y, -1f);

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
            .TransformPoint(weaponAnchorPoint.x, weaponAnchorPoint.y, -1f);

        return point;
    }

    //
    // Spawns thread cells on the screen.
    //
    private void GenerateThreadCells(float startY, int cells)
    {
        // Indicates the next Y position for the next generated cell.
        float nextY = startY;

        // Thread cell's anchor points.
        Vector2 threadCellAnchor = new Vector2(0f, -0.95f);
        Vector2 threadCellDestAnchor = new Vector2(0f, 0.95f);
        for (int i = 0; i < cells; i++)
        {
            // Create instance of a thread cell.
            GameObject threadCell = GameObject.Instantiate(baseThreadCell,
                new Vector3(0f, nextY, -1f), Quaternion.identity);

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