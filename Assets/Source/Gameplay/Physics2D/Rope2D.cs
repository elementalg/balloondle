using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        [FormerlySerializedAs("Limits")] public Rope2DArgs m_Args;
        
        private GameObject _ropeCellPrefab;
        
        private Vector2 _ropeCellSize;
        
        public List<GameObject> RopeCells { get; private set; }
        public GameObject GameObjectAttachedToStart { get; private set; }
        public Vector2 StartBodyAnchorPoint { get; private set; }
        
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
                    .Distance(GameObjectAttachedToStart.transform.TransformPoint(StartBodyAnchorPoint),
                        BodyAttachedToEnd.transform.TransformPoint(EndBodyAnchorPoint));

                if (distance > m_Args.m_MaximumDistanceBetweenBodies)
                {
                    Break();
                    return;
                }

                if (GameObjectAttachedToStart.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 
                    m_Args.m_MaximumSupportedVelocity * m_Args.m_MaximumSupportedVelocity)
                {
                    Break();
                    return;
                }

                if (BodyAttachedToEnd.velocity.sqrMagnitude > 
                    m_Args.m_MaximumSupportedVelocity * m_Args.m_MaximumSupportedVelocity)
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

                if (ropeCellBody.velocity.sqrMagnitude > 
                    m_Args.m_MaximumSupportedVelocity * m_Args.m_MaximumSupportedVelocity)
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
        public void AddCellsForJoiningStartToEnd(GameObject start, Vector2 startAnchorPoint, 
            Rigidbody2D end, Vector2 endAnchorPoint)
        {
            ExceptionIfInvalidRopeCell(_ropeCellPrefab);
            
            RemoveAllCells();
            
            StartBodyAnchorPoint = startAnchorPoint;
            EndBodyAnchorPoint = endAnchorPoint;

            AttachJointToStart(start);
            AttachJointToEnd(end);
            
            SpawnCellsBetweenStartAndEnd();
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
            HingeJoint2D hingeJoint2D = connectingPoint.GetComponent<HingeJoint2D>() == null ?
                connectingPoint.AddComponent<HingeJoint2D>() :
                connectingPoint.GetComponent<HingeJoint2D>();

            float jointBreakForce = IsJointOfConnectingPointRelatedToAnEndPoint(connectingPoint)
                ? m_Args.m_EndBodiesJointBreakForce
                : m_Args.m_RopeCellsJointBreakForce;
            float jointBreakTorque = IsJointOfConnectingPointRelatedToAnEndPoint(connectingPoint)
                ? m_Args.m_EndBodiesJointBreakTorque
                : m_Args.m_RopeCellsJointBreakTorque;
            
            hingeJoint2D.enableCollision = enableCollision;
            hingeJoint2D.autoConfigureConnectedAnchor = enableCollision;
            hingeJoint2D.breakForce = jointBreakForce;
            hingeJoint2D.breakTorque = jointBreakTorque;

            DistanceJoint2D distanceJoint2D = connectingPoint.GetComponent<DistanceJoint2D>() == null ?
                connectingPoint.AddComponent<DistanceJoint2D>() :
                connectingPoint.GetComponent<DistanceJoint2D>();
            
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

            DistanceJoint2D distanceJoint2D = body2D.gameObject.GetComponent<DistanceJoint2D>() == null ?
                body2D.gameObject.AddComponent<DistanceJoint2D>() :
                body2D.gameObject.GetComponent<DistanceJoint2D>();
            
            distanceJoint2D.anchor = EndBodyAnchorPoint;
            distanceJoint2D.connectedBody = GameObjectAttachedToStart.GetComponent<Rigidbody2D>();
            distanceJoint2D.connectedAnchor = StartBodyAnchorPoint;
            distanceJoint2D.enableCollision = true;
            distanceJoint2D.autoConfigureDistance = false;
            distanceJoint2D.distance = m_Args.m_Length;
            distanceJoint2D.maxDistanceOnly = true;
            distanceJoint2D.breakForce = m_Args.m_JointBetweenEndsBreakForce;
        }
        
        private void SpawnCellsBetweenStartAndEnd()
        {
            Vector3 startPosition = GameObjectAttachedToStart.transform.TransformPoint(StartBodyAnchorPoint);
            Vector3 endPosition = BodyAttachedToEnd.transform.TransformPoint(EndBodyAnchorPoint);

            float distance = Vector2.Distance(startPosition, endPosition);

            Vector2 direction = endPosition - startPosition;
            direction.Normalize();

            if (m_Args.m_MaximumDistanceBetweenBodies / distance >= 2f)
            {
                SpawnCellsInZigZag(startPosition, endPosition, direction, 
                    m_Args.m_MaximumDistanceBetweenBodies, distance); 
            }
            else
            {
                SpawnCellsInLine(startPosition, direction, m_Args.m_MaximumDistanceBetweenBodies);
            }
        }

        private void SpawnCellsInZigZag(Vector2 start, Vector2 end, Vector2 direction, float length, float distance)
        {
            if (Mathf.CeilToInt(length / distance) < 2)
            {
                throw new InvalidOperationException(
                    "ZigZag spawning method requires at least a length twice larger than the distance.");
            }
            
            // + 2 -> include start and end half segments.
            int halfSegments = Mathf.CeilToInt(length / distance) * 2 + 2;
            float halfSegmentBase = distance / halfSegments;
            float halfSegmentHypotenuse = length / halfSegments;
            float zigZagHeight =
                Mathf.Sqrt(halfSegmentHypotenuse * halfSegmentHypotenuse - halfSegmentBase * halfSegmentBase);
            int zigZagSign = +1;
            
            Vector2 normal = Vector2.Perpendicular(direction);
            
            Vector2 zigZagVertex = new Vector2(start.x, start.y);
            Vector2 previousPoint = new Vector2(start.x, start.y);
            Vector2 lineDirection;
            for (int i = 0; i < halfSegments; i += 2, zigZagSign *= -1)
            { 
                previousPoint.Set(zigZagVertex.x, zigZagVertex.y);

                zigZagVertex = (start + (i + 1) * halfSegmentBase * direction) + zigZagHeight * normal * zigZagSign;
                lineDirection = (zigZagVertex - previousPoint).normalized;
                SpawnCellsInLine(previousPoint, lineDirection,
                    (i == 0) ? halfSegmentHypotenuse : halfSegmentHypotenuse * 2);
            }
            
            // Adapt last half segment's length to the distance between the last zig zag vertex and the end point.
            lineDirection = (end - zigZagVertex).normalized;
            SpawnCellsInLine(zigZagVertex, lineDirection, Vector2.Distance(end, zigZagVertex));
        }

        private void SpawnCellsInLine(Vector2 start, Vector2 direction, float distance)
        {
            ulong cells = (ulong) Mathf.Ceil(distance / Mathf.Abs(_ropeCellSize.x));
            // Fill in with cells the space created due to the overlapping of cells.
            Vector2 cellPosition = new Vector2(start.x, start.y);

            for (ulong cell = 0ul; cell < cells; cell++)
            {
                cellPosition.Set(start.x + (_ropeCellSize.x * cell * direction.x), 
                    start.y + (_ropeCellSize.y * cell * direction.y));
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
                hingeJoint2D.anchor = StartBodyAnchorPoint;
                distanceJoint2D.anchor = StartBodyAnchorPoint;
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