using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Gameplay.Physics2D
{
    /// <summary>
    /// Rope made up of cells which connects two gameObjects with certain constraints.
    ///
    /// The rope can get broken by the following reasons:
    /// * A joint contained within any of the end's GameObject, or, within any of the cells, has been broken.
    /// * The velocity of the start, or the end, has exceeded the maximum supported velocity.
    /// * The distance, between the start and the end, has exceeded the maximum allowed distance. 
    /// </summary>
    public class Rope2D : MonoBehaviour
    {
        public IRope2DCustomDestructor CustomDestructor;
        
        /// <summary>
        /// Maximum velocity supported before the rope proceeds to break.
        /// </summary>
        private const float MaximumSupportedVelocity = 100f;
        
        private GameObject _ropeCellPrefab;
        
        private Vector2 _ropeCellSize;
        
        /// <summary>
        /// Distance constraint between the start gameObject and the end one. If the limit is broken, the
        /// rope proceeds to break.
        /// </summary>
        private float _maximumDistanceBetweenStartAndEnd = 4.75f;

        public List<GameObject> RopeCells { get; private set; }
        public GameObject GameObjectAttachedToStart { get; private set; }
        public Vector2 StartGameObjectAnchorPoint { get; private set; }
        
        public Rigidbody2D BodyAttachedToEnd { get; private set; }
        public Vector2 EndBodyAnchorPoint { get; private set; }
        
        /// <summary>
        /// Prefab used for instantiating a rope cell.
        ///
        /// <exception cref="InvalidOperationException">if trying to set
        /// a GameObject which is null, or,does not contain a <see cref="SpriteRenderer"/>,
        /// a <see cref="Rigidbody2D"/>, a <see cref="BoxCollider2D"/>.</exception>
        /// </summary>
        public GameObject RopeCellPrefab
        {
            get => _ropeCellPrefab;
            set
            {
                ExceptionIfInvalidRopeCell(value);

                _ropeCellSize = value.GetComponent<SpriteRenderer>().sprite.bounds.size;
                _ropeCellPrefab = value;
            }
        }

        /// <summary>
        /// Force which proceeds to break any of the joints contained within the start GameObject,
        /// or, within any of the rope cells.
        /// </summary>
        public float EndBodiesJointBreakForce { get; set; } = 2f;
        
        /// <summary>
        /// Torque which proceeds to break the <see cref="HingeJoint2D"/> used within the start GameObject,
        /// and every rope cell.
        /// </summary>
        public float EndBodiesJointBreakTorque { get; set; } = 2f;

        public float RopeCellsJointBreakForce { get; set; } = 1f;
        public float RopeCellsJointBreakTorque { get; set; } = 1f;
        
        /// <summary>
        /// Force which proceeds to break the <see cref="DistanceJoint2D"/> connecting the start and the end.
        /// </summary>
        public float JointBetweenEndsBreakForce { get; set; } = 1000f;

        private void OnEnable()
        {
            RopeCells = new List<GameObject>();
        }

        private void OnDisable()
        {
            RemoveAllCells();
        }

        /// <summary>
        /// Assures constraints are met.
        /// </summary>
        private void FixedUpdate()
        {
            if (GameObjectAttachedToStart != null && BodyAttachedToEnd != null)
            {
                float distance = Vector2
                    .Distance(GameObjectAttachedToStart.transform.position,
                        BodyAttachedToEnd.transform.position);

                if (distance > _maximumDistanceBetweenStartAndEnd)
                {
                    Break();
                    return;
                }

                if (GameObjectAttachedToStart.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 
                    MaximumSupportedVelocity * MaximumSupportedVelocity)
                {
                    Break();
                    return;
                }

                if (BodyAttachedToEnd.velocity.sqrMagnitude > 
                    MaximumSupportedVelocity * MaximumSupportedVelocity)
                {
                    Break();
                }
            }

            BreakRopeJointIfQuarterRopeCellsExceedMaximumVelocity();
        }
        
        /// <summary>
        /// Removes the rope cells added to the Scene, and the added joints to the start and end.
        /// Finally, it proceeds to self-destruct.
        ///
        /// First it checks if there's a CustomDestroyer assigned, otherwise it proceeds to call Destroy.
        /// </summary>
        public void Break()
        {
            RemoveAllCells();

            RemoveJointsFromEnds();

            if (CustomDestructor != null)
            {
                CustomDestructor.OnBreakRope();
            }
            else
            {
                Destroy(gameObject); // Self destruct.
            }
        }
        
        /// <summary>
        /// Detaches the joint attached to the start, and the body attached to the end of the rope. Then proceeds to
        /// destroy all rope cells.
        /// </summary>
        private void RemoveAllCells()
        {
            if (RopeCells.Count < 1)
            {
                return;
            }

            if (GameObjectAttachedToStart != null)
            {
                AttachGameObjectToRigidBody(GameObjectAttachedToStart, null);
            }

            foreach (GameObject ropeCell in RopeCells)
            {
                AttachGameObjectToRigidBody(ropeCell, null);
                Destroy(ropeCell);
            }
            
            RopeCells.Clear();
        }
        
        private void RemoveJointsFromEnds()
        {
            if (GameObjectAttachedToStart.GetComponent<DistanceJoint2D>() != null)
            {
                Destroy(GameObjectAttachedToStart.GetComponent<DistanceJoint2D>());
            }

            if (GameObjectAttachedToStart.GetComponent<HingeJoint2D>() != null)
            {
                Destroy(GameObjectAttachedToStart.GetComponent<HingeJoint2D>());
            }
            
            if (BodyAttachedToEnd.GetComponent<DistanceJoint2D>() != null)
            {
                Destroy(BodyAttachedToEnd.GetComponent<DistanceJoint2D>());
            }
        }

        /// <summary>
        /// Checks the first, second, and third quarter rope cell's velocity. If their velocity exceeds the maximum, the rope
        /// will break.
        /// </summary>
        private void BreakRopeJointIfQuarterRopeCellsExceedMaximumVelocity()
        {
            // Check only the first, second, and third quarter.
            // Thus, only check when the amount of rope cells is bigger than 1.
            for (int i = 0; i <= 2 && RopeCells.Count > 1; i++)
            {
                int index = ((1 + i) * RopeCells.Count) / 4;
                Rigidbody2D ropeCellBody = RopeCells[index].GetComponent<Rigidbody2D>();

                if (ropeCellBody.velocity.sqrMagnitude > MaximumSupportedVelocity * MaximumSupportedVelocity)
                {
                    Break();
                    return;
                }
            }
        }
        
        /// <summary>
        /// Creates a rope long enough to fill the existing distance between the start and the end.
        /// This rope will break if the distance between the start and the end exceeds the established maximum.
        /// </summary>
        /// <param name="start">GameObject attached to the start of the rope.</param>
        /// <param name="startAnchorPoint"></param>
        /// <param name="end">GameObject attached to the end of the rope.</param>
        /// <param name="endAnchorPoint"></param>
        /// <param name="maximumDistanceBetweenStartAndEnd"></param>
        public void AddCellsForJoiningStartToEnd(GameObject start, Vector2 startAnchorPoint,
            Rigidbody2D end, Vector2 endAnchorPoint, float maximumDistanceBetweenStartAndEnd)
        {
            ExceptionIfInvalidRopeCell(_ropeCellPrefab);
            
            RemoveAllCells();
            
            StartGameObjectAnchorPoint = startAnchorPoint;
            EndBodyAnchorPoint = endAnchorPoint;
            _maximumDistanceBetweenStartAndEnd = maximumDistanceBetweenStartAndEnd;

            AttachJointToStart(start);
            AttachJointToEnd(end);
            
            FillWithCellsDistanceBetweenStartAndEnd();
        }

        private void ExceptionIfInvalidRopeCell(GameObject prefab)
        {
            if (prefab == null)
            {
                throw new InvalidOperationException("Rope requires a cell prefab.");
            }

            if (prefab.GetComponent<SpriteRenderer>() == null)
            {
                throw new InvalidOperationException("Rope cell's prefab must contain a Sprite component.");
            }
            
            if (prefab.GetComponent<Rigidbody2D>() == null)
            {
                throw new InvalidOperationException("Rope cell's prefab must contain a Rigidbody2D component.");
            }
            
            if (prefab.GetComponent<BoxCollider2D>() == null)
            {
                throw new InvalidOperationException(
                    "Rope cell's prefab must contain a BoxCollider2D component.");
            }
        }
        
        /// <summary>
        /// Detaches previous joint from the start of the rope, and attaches the new joint to the start of the rope.
        /// </summary>
        /// <param name="startGameObject">Joint being attached to the rope's start.</param>
        private void AttachJointToStart(GameObject startGameObject)
        {
            if (GameObjectAttachedToStart != null)
            {
                Destroy(GameObjectAttachedToStart.GetComponent<Rope2DJointController>());
                AttachGameObjectToRigidBody(GameObjectAttachedToStart, null);
            }

            
            GameObjectAttachedToStart = startGameObject;
            // Enable collision with attached rope cell, in order to avoid the cell passing through the object.
            AddJointsToConnectingPoint(GameObjectAttachedToStart, true);
            
            Rope2DJointController rope2DJointController = GameObjectAttachedToStart.AddComponent<Rope2DJointController>();
            rope2DJointController.RopeInstance = this;
            
            if (RopeCells.Count > 0)
            {
                AttachGameObjectToRigidBody(GameObjectAttachedToStart, RopeCells[0].GetComponent<Rigidbody2D>());
            }
        }

        /// <summary>
        /// Adds the required joints to the start GameObject, and to every rope cell.
        /// </summary>
        /// <param name="connectingPoint">Instance of the start GameObject, or of a rope cell.</param>
        /// <param name="enableCollision">Whether or not, the connectingPoint should collide with the attached
        /// body.</param>
        private void AddJointsToConnectingPoint(GameObject connectingPoint, bool enableCollision)
        {
            HingeJoint2D hingeJoint2D;

            if (connectingPoint.GetComponent<HingeJoint2D>() == null)
            {
                hingeJoint2D = connectingPoint.AddComponent<HingeJoint2D>();
            }
            else
            {
                hingeJoint2D = connectingPoint.GetComponent<HingeJoint2D>();
            }

            float jointBreakForce = IsJointOfConnectingPointRelatedToAnEndPoint(connectingPoint)
                ? EndBodiesJointBreakForce
                : RopeCellsJointBreakForce;
            float jointBreakTorque = IsJointOfConnectingPointRelatedToAnEndPoint(connectingPoint)
                ? EndBodiesJointBreakTorque
                : RopeCellsJointBreakTorque;
            
            hingeJoint2D.enableCollision = enableCollision;
            hingeJoint2D.autoConfigureConnectedAnchor = enableCollision;
            hingeJoint2D.breakForce = jointBreakForce;
            hingeJoint2D.breakTorque = jointBreakTorque;

            DistanceJoint2D distanceJoint2D;

            if (connectingPoint.GetComponent<DistanceJoint2D>() == null)
            {
                distanceJoint2D = connectingPoint.AddComponent<DistanceJoint2D>();
            }
            else
            {
                distanceJoint2D = connectingPoint.GetComponent<DistanceJoint2D>();
            }
            
            distanceJoint2D.enableCollision = enableCollision;
            distanceJoint2D.autoConfigureConnectedAnchor = true;
            distanceJoint2D.autoConfigureDistance = true;
            distanceJoint2D.maxDistanceOnly = false;
            distanceJoint2D.breakForce = jointBreakForce;
            distanceJoint2D.breakTorque = jointBreakTorque;
        }

        private bool IsJointOfConnectingPointRelatedToAnEndPoint(GameObject connectingPoint)
        {
            if (connectingPoint == GameObjectAttachedToStart)
            {
                return true;
            }

            if (RopeCells.Count > 0)
            {
                return connectingPoint == RopeCells[RopeCells.Count - 1];
            }

            return false;
        }

        /// <summary>
        /// Detaches previous body from the end of the rope, and attaches the new body to the end of the rope.
        /// </summary>
        /// <param name="body2D">Body being attached to the rope's end.</param>
        private void AttachJointToEnd(Rigidbody2D body2D)
        {
            BodyAttachedToEnd = body2D;
            
            if (RopeCells.Count > 0)
            {
                AttachGameObjectToRigidBody(RopeCells[RopeCells.Count - 1], BodyAttachedToEnd);
            }

            DistanceJoint2D distanceJoint2D;
            
            if (body2D.gameObject.GetComponent<DistanceJoint2D>() == null)
            {
                distanceJoint2D = body2D.gameObject.AddComponent<DistanceJoint2D>();
            }
            else
            {
                distanceJoint2D = body2D.gameObject.GetComponent<DistanceJoint2D>();
            }
            
            distanceJoint2D.connectedBody = GameObjectAttachedToStart.GetComponent<Rigidbody2D>();
            distanceJoint2D.enableCollision = true;
            distanceJoint2D.maxDistanceOnly = true;
            distanceJoint2D.autoConfigureDistance = true;
            distanceJoint2D.breakForce = JointBetweenEndsBreakForce;
        }
        
        private void FillWithCellsDistanceBetweenStartAndEnd()
        {
            Vector3 startPosition = GameObjectAttachedToStart.transform.TransformPoint(StartGameObjectAnchorPoint);
            Vector3 endPosition = BodyAttachedToEnd.transform.TransformPoint(EndBodyAnchorPoint);

            float distance = Vector2.Distance(startPosition, endPosition);
            Vector2 direction = endPosition - startPosition;
            direction.Normalize();

            ulong cells = (ulong) Mathf.Ceil(Mathf.Abs(distance) / Mathf.Abs(_ropeCellSize.x));
            // Fill in with cells the space created due to the overlapping of cells.
            cells = cells + (cells / 4);

            Vector3 cellPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z);

            Vector2 cellOverlap = new Vector2(_ropeCellSize.x * 0.125f * direction.x,
                _ropeCellSize.y * 0.125f * direction.y);
            
            for (ulong cell = 0ul; cell < cells; cell++)
            {
                cellPosition.Set(startPosition.x + (_ropeCellSize.x * cell * direction.x) - ( cellOverlap.x * cell), 
                    startPosition.y + (_ropeCellSize.y * cell * direction.y) - (cellOverlap.y * cell), startPosition.z);
                AddCellAtPosition(cellPosition);
            }
        }
        
        private void AddCellAtPosition(Vector3 ropeCellPosition)
        {
            Transform ropeTransform = transform;
            GameObject ropeCell = Instantiate(_ropeCellPrefab,
                ropeTransform.InverseTransformPoint(ropeCellPosition), Quaternion.identity, ropeTransform);

            // Disable collision between rope cells.
            AddJointsToConnectingPoint(ropeCell, true);
            Rope2DJointController rope2DJointController = ropeCell.AddComponent<Rope2DJointController>();
            rope2DJointController.RopeInstance = this;
            
            RopeCells.Add(ropeCell);

            SynchronizeJointsAfterAddition();

            ropeCell.AddComponent<Rope2DBreakerWhenTraversed>();
        }

        /// <summary>
        /// To be called strictly after a new rope cell has been added to the Rope's transform.
        /// </summary>
        private void SynchronizeJointsAfterAddition()
        {
            if (RopeCells.Count == 1)
            {
                if (GameObjectAttachedToStart != null)
                {
                    AttachGameObjectToRigidBody(GameObjectAttachedToStart, RopeCells[0].GetComponent<Rigidbody2D>());
                }    
            }

            GameObject lastCell = RopeCells[RopeCells.Count - 1];
            if (BodyAttachedToEnd != null)
            {
                AttachGameObjectToRigidBody(lastCell, BodyAttachedToEnd);
            }
            else
            {
                AttachGameObjectToRigidBody(lastCell, null);
            }

            if (RopeCells.Count > 1)
            {
                GameObject penultimateCell = RopeCells[RopeCells.Count - 2];
                
                AttachGameObjectToRigidBody(penultimateCell, lastCell.GetComponent<Rigidbody2D>());
            }
        }

        private void AttachGameObjectToRigidBody(GameObject source, Rigidbody2D destination)
        {
            bool enableJoints = destination != null;
            
            HingeJoint2D hingeJoint2D = source.GetComponent<HingeJoint2D>();
            hingeJoint2D.enabled = enableJoints;
            hingeJoint2D.connectedBody = destination;

            DistanceJoint2D distanceJoint2D = source.GetComponent<DistanceJoint2D>();
            distanceJoint2D.enabled = enableJoints;
            distanceJoint2D.connectedBody = destination;
            
            if (source == GameObjectAttachedToStart)
            {
                hingeJoint2D.anchor = StartGameObjectAnchorPoint;
                distanceJoint2D.anchor = StartGameObjectAnchorPoint;
                return;
            }

            hingeJoint2D.anchor = Vector2.zero;
            hingeJoint2D.connectedAnchor = Vector2.zero;
            distanceJoint2D.anchor = Vector2.zero;
            distanceJoint2D.connectedAnchor = Vector2.zero;

            if (destination == BodyAttachedToEnd)
            {
                hingeJoint2D.connectedAnchor = EndBodyAnchorPoint;
                distanceJoint2D.connectedAnchor = EndBodyAnchorPoint;
            }
        }
    }
}