using System;
using System.Collections.Generic;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Balloondle.Gameplay
{
    public class RopeVisualizer : MonoBehaviour
    {
        private const float MinimalSafeDistanceBetweenPoints = 0.05f;
        
        private GameObject _ropeSpriteShape;
        private Rope2D _rope2D;
        private List<Transform> _ropePoints;

        private SpriteShapeController _spriteShapeController;
        
        public GameObject ropeSpriteShapePrefab { get; set; }
        
        public void VisualizeRope()
        {
            _rope2D = GetComponent<Rope2D>();

            _ropePoints = new List<Transform>();
            _ropePoints.Add(_rope2D.gameObjectAttachedToStart.transform);

            for (int cell = 0; cell < _rope2D.ropeCells.Count; cell++)
            {
                _ropePoints.Add(_rope2D.ropeCells[cell].transform);
            }
            
            _ropePoints.Add(_rope2D.bodyAttachedToEnd.transform);

            _ropeSpriteShape = Instantiate(ropeSpriteShapePrefab, Vector3.zero, Quaternion.identity);
            
            _spriteShapeController = _ropeSpriteShape.GetComponent<SpriteShapeController>();
        }

        public void Update()
        {
            if (_rope2D != null)
            {
                _spriteShapeController.spline.Clear();

                for (int i = 0; i < _ropePoints.Count; i++)
                {
                    Transform pointTransform = _ropePoints[i];

                    Vector3 pointPosition;
                    if (i == 0)
                    {
                        pointPosition = pointTransform.TransformPoint(_rope2D.startGameObjectAnchorPoint);
                    }
                    else if (i == _ropePoints.Count - 1)
                    {
                        pointPosition = pointTransform.TransformPoint(_rope2D.endBodyAnchorPoint);
                    }
                    else
                    {
                        pointPosition = pointTransform.position;
                    }
                    
                    if (i % 2 == 0)
                    {
                        SafeSequentialInsertPointIntoSpline(i / 2, pointPosition);

                        if (i > 0)
                        {
                            _spriteShapeController.spline
                                .SetRightTangent(i / 2, _ropePoints[i - 1].transform.position);
                        }
                    }
                    else
                    {
                        if (i == _ropePoints.Count - 1)
                        {
                            SafeSequentialInsertPointIntoSpline((i / 2) + 1, pointPosition);
                            return;
                        }
                        
                        _spriteShapeController.spline.SetLeftTangent(i / 2, pointPosition);
                    }
                }
                
                _spriteShapeController.RefreshSpriteShape();
            }
        }

        /// <summary>
        /// <see cref="Rope2D"/> destroys the container GameObject when the rope has been broken. Thus, the rope's
        /// sprite shape must be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (_ropeSpriteShape != null)
            {
                Destroy(_ropeSpriteShape);
            }
        }

        /// <summary>
        /// Keeps the sequentially added points at a safe distance,
        /// avoiding 'too close' errors generated by the SpriteShape.
        ///
        /// This method only assures a safe distance when points are added always to the end, and not in between.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="position"></param>
        private void SafeSequentialInsertPointIntoSpline(int index, Vector3 position)
        {
            if (index < _spriteShapeController.spline.GetPointCount() - 1)
            {
                throw new InvalidOperationException("Point is not being inserted sequentially.");
            }
            
            if (index > 0)
            {
                Vector3 previousPoint = _spriteShapeController.spline.GetPosition(index - 1);
                // Check distance with the start of the spline, due to the position validating logic of Spline.
                Vector3 originPoint = _spriteShapeController.spline.GetPosition(0); 
                
                float squaredDistanceFromPreviousPoint = Vector3.SqrMagnitude(previousPoint - position);
                float squaredDistanceFromOriginPoint = Vector3.SqrMagnitude(originPoint - position);
                
                Vector3 safeDirection = (position - originPoint).normalized + (position - previousPoint).normalized;

                if (squaredDistanceFromPreviousPoint <
                    MinimalSafeDistanceBetweenPoints * MinimalSafeDistanceBetweenPoints 
                    || squaredDistanceFromOriginPoint <
                    MinimalSafeDistanceBetweenPoints * MinimalSafeDistanceBetweenPoints)
                {
                    position += MinimalSafeDistanceBetweenPoints * safeDirection.normalized;
                }
            }
            
            _spriteShapeController.spline.InsertPointAt(index, position);
        }
    }
}