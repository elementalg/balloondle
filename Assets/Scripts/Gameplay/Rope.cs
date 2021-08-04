using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class Rope : MonoBehaviour
    {
        private const float MaximumSupportedVelocity = 100f;
        private GameObject _ropeCellPrefab;
        
        private List<GameObject> _cells;
        private Vector2 _ropeCellSize;

        private Vector2 _startAnchorPoint;
        private Vector2 _endAnchorPoint;
        
        private GameObject _gameObjectAttachedToStart;
        private Rigidbody2D _bodyAttachedToEnd;
        
        private float _maximumDistanceBetweenStartAndEnd = 4.75f;

        public GameObject ropeCellPrefab
        {
            get => _ropeCellPrefab;
            set
            {
                ExceptionIfInvalidRopeCell(value);

                _ropeCellSize = value.GetComponent<SpriteRenderer>().sprite.bounds.size;
                _ropeCellPrefab = value;
            }
        }

        public float jointBreakForce { get; set; } = 2f;
        public float jointBreakTorque { get; set; } = 2f;
        public float jointBetweenEndsBreakForce { get; set; } = 1000f;

        private void OnEnable()
        {
            _cells = new List<GameObject>();
        }

        private void OnDisable()
        {
            RemoveAllCells();
        }
        
        private void FixedUpdate()
        {
            if (_gameObjectAttachedToStart != null && _bodyAttachedToEnd != null)
            {
                float distance = Vector2
                    .Distance(_gameObjectAttachedToStart.transform.position, _bodyAttachedToEnd.transform.position);

                if (distance > _maximumDistanceBetweenStartAndEnd)
                {
                    OnRopeJointBreak();
                    return;
                }

                if (_gameObjectAttachedToStart.GetComponent<Rigidbody2D>().velocity.magnitude > MaximumSupportedVelocity)
                {
                    Debug.Log($"GameObject: Velocity {_gameObjectAttachedToStart.GetComponent<Rigidbody2D>().velocity.magnitude}");
                    OnRopeJointBreak();
                    return;
                }

                if (_bodyAttachedToEnd.velocity.magnitude > MaximumSupportedVelocity)
                {
                    Debug.Log($"Body: Velocity {_bodyAttachedToEnd.velocity.magnitude}");
                    OnRopeJointBreak();
                    return;
                }
            }
        }
        
        public void OnRopeJointBreak()
        {
            RemoveAllCells();

            RemoveJointsFromEnds();

            Destroy(gameObject); // Self destruct.
        }
        
        /// <summary>
        /// Detaches the joint attached to the start, and the body attached to the end of the rope. Then proceeds to
        /// destroy all rope cells.
        /// </summary>
        private void RemoveAllCells()
        {
            if (_cells.Count < 1)
            {
                return;
            }

            if (_gameObjectAttachedToStart != null)
            {
                AttachGameObjectToRigidBody(_gameObjectAttachedToStart, null);
            }

            foreach (GameObject ropeCell in _cells)
            {
                AttachGameObjectToRigidBody(ropeCell, null);
                Destroy(ropeCell);
            }
            
            _cells.Clear();
        }
        
        private void RemoveJointsFromEnds()
        {
            if (_gameObjectAttachedToStart.GetComponent<DistanceJoint2D>() != null)
            {
                Destroy(_gameObjectAttachedToStart.GetComponent<DistanceJoint2D>());
            }

            if (_gameObjectAttachedToStart.GetComponent<HingeJoint2D>() != null)
            {
                Destroy(_gameObjectAttachedToStart.GetComponent<HingeJoint2D>());
            }
            
            if (_bodyAttachedToEnd.GetComponent<DistanceJoint2D>() != null)
            {
                Destroy(_bodyAttachedToEnd.GetComponent<DistanceJoint2D>());
            }
        }
        
        public void AddCellsForJoiningStartToEnd(GameObject start, Vector2 startAnchorPoint,
            Rigidbody2D end, Vector2 endAnchorPoint, float maximumDistanceBetweenStartAndEnd)
        {
            ExceptionIfInvalidRopeCell(_ropeCellPrefab);
            
            RemoveAllCells();
            
            _startAnchorPoint = startAnchorPoint;
            _endAnchorPoint = endAnchorPoint;
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
            if (_gameObjectAttachedToStart != null)
            {
                Destroy(_gameObjectAttachedToStart.GetComponent<RopeJointController>());
                AttachGameObjectToRigidBody(_gameObjectAttachedToStart, null);
            }

            
            _gameObjectAttachedToStart = startGameObject;
            // Enable collision with attached rope cell, in order to avoid the cell passing through the object.
            AddJointsToConnectingPoint(_gameObjectAttachedToStart, true);
            
            RopeJointController ropeJointController = _gameObjectAttachedToStart.AddComponent<RopeJointController>();
            ropeJointController.RopeInstance = this;
            
            if (_cells.Count > 0)
            {
                AttachGameObjectToRigidBody(_gameObjectAttachedToStart, _cells[0].GetComponent<Rigidbody2D>());
            }
        }

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
        }

        /// <summary>
        /// Detaches previous body from the end of the rope, and attaches the new body to the end of the rope.
        /// </summary>
        /// <param name="body2D">Body being attached to the rope's end.</param>
        private void AttachJointToEnd(Rigidbody2D body2D)
        {
            _bodyAttachedToEnd = body2D;
            
            if (_cells.Count > 0)
            {
                AttachGameObjectToRigidBody(_cells[_cells.Count - 1], _bodyAttachedToEnd);
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
            
            distanceJoint2D.connectedBody = _gameObjectAttachedToStart.GetComponent<Rigidbody2D>();
            distanceJoint2D.enableCollision = true;
            distanceJoint2D.maxDistanceOnly = true;
            distanceJoint2D.autoConfigureDistance = true;
            distanceJoint2D.breakForce = jointBetweenEndsBreakForce;
        }
        
        private void FillWithCellsDistanceBetweenStartAndEnd()
        {
            Vector3 startPosition = _gameObjectAttachedToStart.transform.TransformPoint(_startAnchorPoint);
            Vector3 endPosition = _bodyAttachedToEnd.transform.TransformPoint(_endAnchorPoint);

            float distance = Vector2.Distance(startPosition, endPosition);
            Vector2 direction = endPosition - startPosition;
            direction.Normalize();

            ulong cells = (ulong) Mathf.Ceil(Mathf.Abs(distance) / Mathf.Abs(_ropeCellSize.x));
            cells = cells + (cells / 4);

            Vector3 cellPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z);
            
            for (ulong cell = 0ul; cell < cells; cell++)
            {
                cellPosition.Set(startPosition.x + (_ropeCellSize.x * cell * direction.x) - (_ropeCellSize.x * 0.125f * cell * direction.x), 
                    startPosition.y + (_ropeCellSize.y * cell * direction.y) - (_ropeCellSize.y * 0.125f * cell * direction.y), startPosition.z);
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
            RopeJointController ropeJointController = ropeCell.AddComponent<RopeJointController>();
            ropeJointController.RopeInstance = this;
            
            _cells.Add(ropeCell);

            SynchronizeJointsAfterAddition();
        }

        /// <summary>
        /// To be called strictly after a new rope cell has been added to the Rope's transform.
        /// </summary>
        private void SynchronizeJointsAfterAddition()
        {
            if (_cells.Count == 1)
            {
                if (_gameObjectAttachedToStart != null)
                {
                    AttachGameObjectToRigidBody(_gameObjectAttachedToStart, _cells[0].GetComponent<Rigidbody2D>());
                }    
            }

            GameObject lastCell = _cells[_cells.Count - 1];
            if (_bodyAttachedToEnd != null)
            {
                AttachGameObjectToRigidBody(lastCell, _bodyAttachedToEnd);
            }
            else
            {
                AttachGameObjectToRigidBody(lastCell, null);
            }

            if (_cells.Count > 1)
            {
                GameObject penultimateCell = _cells[_cells.Count - 2];
                
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
            
            if (source == _gameObjectAttachedToStart)
            {
                hingeJoint2D.anchor = _startAnchorPoint;
                distanceJoint2D.anchor = _startAnchorPoint;
                return;
            }

            hingeJoint2D.anchor = Vector2.zero;
            hingeJoint2D.connectedAnchor = Vector2.zero;
            distanceJoint2D.anchor = Vector2.zero;
            distanceJoint2D.connectedAnchor = Vector2.zero;

            if (destination == _bodyAttachedToEnd)
            {
                hingeJoint2D.connectedAnchor = _endAnchorPoint;
                distanceJoint2D.connectedAnchor = _endAnchorPoint;
            }
        }
    }
}