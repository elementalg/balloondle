using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Generator
{
    public class Rope : MonoBehaviour
    {
        [SerializeField, Tooltip("Rope cell's prefab.")]
        private GameObject m_RopeCellPrefab;
        
        private List<Joint2D> _cells;
        private Vector2 _ropeCellSize;

        private Joint2D _jointAttachedToStart;
        private Rigidbody2D _bodyAttachedToEnd;

        private void OnEnable()
        {
            if (m_RopeCellPrefab == null)
            {
                throw new InvalidOperationException("Rope requires a cell prefab.");
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
            
            _cells = new List<Joint2D>();

            _ropeCellSize = m_RopeCellPrefab.GetComponent<BoxCollider2D>().size;
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

            if (_cells.Count > 0)
            {
                Vector3 lastRopeCellPosition = _cells[_cells.Count - 1].transform.localPosition;
                ropeCellPosition.Set(lastRopeCellPosition.x, lastRopeCellPosition.y + _ropeCellSize.y, 0f);
            }

            GameObject ropeCell = Instantiate(m_RopeCellPrefab, ropeCellPosition, Quaternion.identity, transform);
            
            _cells.Add(ropeCell.GetComponent<Joint2D>());

            SynchronizeJointsAfterAddition();
        }

        /// <summary>
        /// To be called strictly after a new rope cell has been added to the Rope's transform.
        /// </summary>
        private void SynchronizeJointsAfterAddition()
        {
            if (_cells.Count == 1)
            {
                if (_jointAttachedToStart != null)
                {
                    _jointAttachedToStart.connectedBody = _cells[0].GetComponent<Rigidbody2D>();
                }    
            }
            
            if (_bodyAttachedToEnd != null)
            {
                _cells[_cells.Count - 1].connectedBody = _bodyAttachedToEnd;
            }

            if (_cells.Count > 1)
            {
                _cells[_cells.Count - 2].connectedBody = _cells[_cells.Count - 1].GetComponent<Rigidbody2D>();
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
            
            Joint2D ropeCell = _cells[_cells.Count - 1];

            if (_cells.Count == 1)
            {
                if (_jointAttachedToStart != null)
                {
                    _jointAttachedToStart.connectedBody = null;
                }

                ropeCell.connectedBody = null;
            }
            else
            {
                _cells[_cells.Count - 2].connectedBody = _bodyAttachedToEnd;
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

            if (_jointAttachedToStart != null)
            {
                _jointAttachedToStart.connectedBody = null;
            }

            foreach (Joint2D ropeCell in _cells)
            {
                ropeCell.connectedBody = null;
                Destroy(ropeCell);
            }
            
            _cells.Clear();
        }
        
        /// <summary>
        /// Detaches previous joint from the start of the rope, and attaches the new joint to the start of the rope.
        /// </summary>
        /// <param name="joint2D">Joint being attached to the rope's start.</param>
        public void AttachJointToStart(Joint2D joint2D)
        {
            if (_jointAttachedToStart != null)
            {
                _jointAttachedToStart.connectedBody = null;
            }

            _jointAttachedToStart = joint2D;

            if (_cells.Count > 0)
            {
                _jointAttachedToStart.connectedBody = _cells[0].GetComponent<Rigidbody2D>();
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
                _cells[_cells.Count - 1].connectedBody = _bodyAttachedToEnd;
            }
        }
    }
}