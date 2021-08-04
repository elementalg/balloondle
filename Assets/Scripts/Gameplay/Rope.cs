using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class Rope : MonoBehaviour
    {
        private const float MaximumDistanceBetweenConnections = 4f;

        [SerializeField, Tooltip("Rope cell's prefab.")]
        private GameObject m_RopeCellPrefab;
        
        private List<GameObject> _cells;
        private Vector2 _ropeCellSize;

        private Vector2 _startAnchorPoint;
        private Vector2 _endAnchorPoint;
        
        private GameObject _gameObjectAttachedToStart;
        private Rigidbody2D _bodyAttachedToEnd;

        private void OnEnable()
        {
            if (m_RopeCellPrefab == null)
            {
                throw new InvalidOperationException("Rope requires a cell prefab.");
            }

            if (m_RopeCellPrefab.GetComponent<SpriteRenderer>() == null)
            {
                throw new InvalidOperationException("Rope cell's prefab must contain a Sprite component.");
            }
            
            if (m_RopeCellPrefab.GetComponent<Rigidbody2D>() == null)
            {
                throw new InvalidOperationException("Rope cell's prefab must contain a Rigidbody2D component.");
            }
            
            if (m_RopeCellPrefab.GetComponent<BoxCollider2D>() == null)
            {
                throw new InvalidOperationException(
                    "Rope cell's prefab must contain a BoxCollider2D component.");
            }
            
            if (m_RopeCellPrefab.GetComponent<Joint2D>() == null)
            {
                throw new InvalidOperationException("Rope cell's prefab must contain a Joint2D component.");
            }
            
            _cells = new List<GameObject>();

            _ropeCellSize = m_RopeCellPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size;
        }

        private void OnDisable()
        {
            _cells.Clear();
        }

        /// <summary>
        /// Adds a new rope cell to the rope. In case there are start and end joints assigned, these will be
        /// attached automatically to the new start/end of the rope.
        /// </summary>
        public void AddCell()
        {
            Vector3 ropeCellPosition = new Vector3(0f, 0f, 0f);

            if (_gameObjectAttachedToStart != null)
            {
                Vector3 startJointPosition = _gameObjectAttachedToStart.transform.position;
                ropeCellPosition
                    .Set(startJointPosition.x, startJointPosition.y - _ropeCellSize.y, startJointPosition.z);
            }
            
            if (_cells.Count > 0)
            {
                Vector3 lastRopeCellPosition = _cells[_cells.Count - 1].transform.position;
                ropeCellPosition.Set(lastRopeCellPosition.x, lastRopeCellPosition.y - _ropeCellSize.y, 0f);
            }

            AddCellAtPosition(ropeCellPosition);
        }

        private void AddCellAtPosition(Vector3 ropeCellPosition)
        {
            GameObject ropeCell = Instantiate(m_RopeCellPrefab, ropeCellPosition, Quaternion.identity);

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
            }

            if (destination == _bodyAttachedToEnd)
            {
                hingeJoint2D.connectedAnchor = _endAnchorPoint;
                distanceJoint2D.connectedAnchor = _endAnchorPoint;
            }
        }
        
        /// <summary>
        /// Removes a cell from the end, thus connecting the new last cell to the stored body, if there's one,
        /// to be connected with the last cell.
        /// </summary>
        public void RemoveCell()
        {
            if (_cells.Count < 1)
            {
                return;
            }
            
            GameObject ropeCell = _cells[_cells.Count - 1];

            if (_cells.Count == 1)
            {
                if (_gameObjectAttachedToStart != null)
                {
                    AttachGameObjectToRigidBody(_gameObjectAttachedToStart, null);
                }
                
                AttachGameObjectToRigidBody(ropeCell, null);
            }
            else
            {
                AttachGameObjectToRigidBody(_cells[_cells.Count - 2], _bodyAttachedToEnd);
            }

            _cells.RemoveAt(_cells.Count - 1);
            Destroy(ropeCell);
        }

        /// <summary>
        /// Detaches the joint attached to the start, and the body attached to the end of the rope. Then proceeds to
        /// destroy all rope cells.
        /// </summary>
        public void RemoveAllCells()
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
        
        /// <summary>
        /// Detaches previous joint from the start of the rope, and attaches the new joint to the start of the rope.
        /// </summary>
        /// <param name="startGameObject">Joint being attached to the rope's start.</param>
        public void AttachJointToStart(GameObject startGameObject)
        {
            if (_gameObjectAttachedToStart != null)
            {
                Destroy(_gameObjectAttachedToStart.GetComponent<RopeJointController>());
                AttachGameObjectToRigidBody(_gameObjectAttachedToStart, null);
            }

            _gameObjectAttachedToStart = startGameObject;

            RopeJointController ropeJointController = _gameObjectAttachedToStart.AddComponent<RopeJointController>();
            ropeJointController.RopeInstance = this;
            
            if (_cells.Count > 0)
            {
                AttachGameObjectToRigidBody(_gameObjectAttachedToStart, _cells[0].GetComponent<Rigidbody2D>());
            }
        }

        /// <summary>
        /// Detaches previous body from the end of the rope, and attaches the new body to the end of the rope.
        /// </summary>
        /// <param name="body2D">Body being attached to the rope's end.</param>
        public void AttachJointToEnd(Rigidbody2D body2D)
        {
            _bodyAttachedToEnd = body2D;
            
            if (_cells.Count > 0)
            {
                AttachGameObjectToRigidBody(_cells[_cells.Count - 1], _bodyAttachedToEnd);
            }
        }
        
        public void AddCellsForJoiningStartToEnd(GameObject start, Vector2 startAnchorPoint,
            Rigidbody2D end, Vector2 endAnchorPoint)
        {
            RemoveAllCells();
            
            AttachJointToStart(start);
            AttachJointToEnd(end);

            _startAnchorPoint = startAnchorPoint;
            _endAnchorPoint = endAnchorPoint;
            Vector3 startPosition = start.transform.TransformPoint(startAnchorPoint);
            Vector3 endPosition = end.transform.TransformPoint(endAnchorPoint);

            float distance = Vector2.Distance(startPosition, endPosition);
            Vector2 direction = endPosition - startPosition;
            direction.Normalize();

            ulong cells = (ulong) Mathf.Ceil(Mathf.Abs(distance) / Mathf.Abs(_ropeCellSize.x));

            Vector3 cellPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z);
            
            for (ulong cell = 0ul; cell < cells; cell++)
            {
                cellPosition.Set(startPosition.x + (_ropeCellSize.x * cell * direction.x), 
                    startPosition.y + (_ropeCellSize.y * cell * direction.y), startPosition.z);
                AddCellAtPosition(cellPosition);
            }
        }

        private void FixedUpdate()
        {
            if (_gameObjectAttachedToStart != null && _bodyAttachedToEnd != null)
            {
                float distance = Vector2
                    .Distance(_gameObjectAttachedToStart.transform.position, _bodyAttachedToEnd.transform.position);

                if (distance > MaximumDistanceBetweenConnections)
                {
                    OnRopeJointBreak();
                }
            }
        }
        
        public void OnRopeJointBreak()
        {
            RemoveAllCells();

            if (_bodyAttachedToEnd.GetComponent<Joint2D>() != null)
            {
                Destroy(_bodyAttachedToEnd.GetComponent<Joint2D>());
            }
        }
    }
}