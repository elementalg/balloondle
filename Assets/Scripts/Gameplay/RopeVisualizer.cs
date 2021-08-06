using Balloondle.Gameplay.Physics2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Balloondle.Gameplay
{
    public class RopeVisualizer : MonoBehaviour
    {
        private const string RopeSpriteShapeName = "RopeSpriteShape";
        
        private GameObject _ropeSpriteShape;
        private Rope2D _rope2D;

        private SpriteShapeController _spriteShapeController;
        
        public GameObject ropeSpriteShapePrefab { get; set; }
        
        public void VisualizeRope()
        {
            _rope2D = GetComponent<Rope2D>();

            _ropeSpriteShape = Instantiate(ropeSpriteShapePrefab, Vector3.zero, Quaternion.identity);
            
            _spriteShapeController = _ropeSpriteShape.GetComponent<SpriteShapeController>();
        }

        public void Update()
        {
            if (_rope2D != null)
            {
                _spriteShapeController.spline.Clear();
                _spriteShapeController.spline.InsertPointAt(0, _rope2D.gameObjectAttachedToStart.transform
                    .TransformPoint(_rope2D.startGameObjectAnchorPoint));
                
                for (int cell = 1; cell <= _rope2D.ropeCells.Count; cell++)
                {
                    _spriteShapeController.spline.InsertPointAt(cell, _rope2D.ropeCells[cell].transform.position);
                }
                
                _spriteShapeController.spline.InsertPointAt(_rope2D.ropeCells.Count + 1, _rope2D.bodyAttachedToEnd.transform
                    .TransformPoint(_rope2D.endBodyAnchorPoint));
                _spriteShapeController.RefreshSpriteShape();
            }
        }
    }
}