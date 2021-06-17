using Balloondle.Shared.Network.Game;
using MLAPI;
using UnityEngine;

namespace Balloondle.Client
{
    /// <summary>
    /// Synchronizes all the instances of HingeJoints2D, assigning the RigidBody2D accordingly to the
    /// game logic.
    /// </summary>
    public class JointPhysicsSynchronizer : MonoBehaviour
    {
        /// <summary>
        /// Start listening to the event 'OnSpawnPlayerBalloonAndWeapon'.
        /// </summary>
        void Start()
        {
            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer
                .GetComponent<NetworkRpcMessages>()
                .OnSpawnPlayerBalloonAndWeapon += SynchronizeJointsOnAttachWeapon;
        }

        /// <summary>
        /// Links the joints two by two.
        /// </summary>
        /// <param name="objectIds">IDs ordered by starting with the balloon, and ending with the weapon.</param>
        public void SynchronizeJointsOnAttachWeapon(ulong ownerId, ulong[] objectIds)
        {
            Debug.Log("Synchronizing joints.");

            Rigidbody2D[] rigidbodies = GameObject.FindObjectsOfType<Rigidbody2D>();
            GameObject[] objects = new GameObject[objectIds.Length];

            // Retrieve the objects which have a RigidBody2D.
            for (int i = 0; i < objectIds.Length; i++)
            {
                for (int j = 0; j < rigidbodies.Length; j++)
                {
                    NetworkObject netObject = rigidbodies[j].gameObject.GetComponent<NetworkObject>();

                    if (objectIds[i] == netObject.NetworkObjectId)
                    {
                        objects[i] = netObject.gameObject;
                        break;
                    }
                }
            }

            // Create a chain of connections between HingeJoint2D and RigidBody2D.
            for (int i = 0; i < objects.Length - 1; i++)
            {
                GameObject currentObject = objects[i];
                GameObject nextObject = objects[i + 1];

                ConnectSourceWithJointsToDestination(currentObject, nextObject);
            }
        }

        /// <summary>
        /// Connects the source's joint with the destination's rigid body.
        /// </summary>
        /// <param name="source">Object with a HingeJoint2D component</param>
        /// <param name="destination">Object with a RigidBody2D component</param>
        private void ConnectSourceWithJointsToDestination(GameObject source, GameObject destination)
        {
            HingeJoint2D hingeJoint2D = source.GetComponent<HingeJoint2D>();

            Rigidbody2D body = destination.GetComponent<Rigidbody2D>();

            if (hingeJoint2D != null && body != null)
            {
                // Update anchors and connected body.
                hingeJoint2D.connectedBody = body;
            }
            else
            {
                Debug.Log($"Source: {source.name} && Destination: {destination.name}");
            }
        }
    }
}
