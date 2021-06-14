using Balloondle.Shared.Network.Game;
using MLAPI;
using UnityEngine;

namespace Balloondle.Client
{
    public class JointPhysicsSynchronizer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer.GetComponent<NetworkRpcMessages>().OnSpawnPlayerBalloonAndWeapon += SynchronizeJointsOnAttachWeapon;
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

            for (int i = 0; i < objects.Length - 1; i++)
            {
                GameObject currentObject = objects[i];
                GameObject nextObject = objects[i + 1];

                ConnectSourceWithJointsToDestination(currentObject, nextObject);
            }
        }

        //
        // Connects the source's joint with the destination's rigid body.
        //
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

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
