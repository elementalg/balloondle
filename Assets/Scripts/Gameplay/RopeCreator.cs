using System;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class RopeCreator : MonoBehaviour
    {
        [SerializeField] private GameObject m_RopeCellPrefab;
        [SerializeField] private GameObject m_RopeCellSpriteShapePrefab;
        
        private const string RopeTag = "Rope";
        
        public void CreateRopeConnectingTwoRigidBodies2D(Rigidbody2D start, Vector2 startAnchor, 
            Rigidbody2D end, Vector2 endAnchor, float maximumDistanceBetweenBodies)
        {
            if (start == end)
            {
                throw new InvalidOperationException("Cannot create a rope connecting the same rigid body.");
            }

            GameObject ropeGameObject = new GameObject
            {
                tag = RopeTag
            };
            
            ropeGameObject.GetComponent<Transform>().position = Vector3.zero;
            Rope2D rope = ropeGameObject.AddComponent<Rope2D>();
            rope.ropeCellPrefab = m_RopeCellPrefab;
            rope.AddCellsForJoiningStartToEnd(start.gameObject, startAnchor,
                end, endAnchor, maximumDistanceBetweenBodies);

            RopeVisualizer ropeVisualizer = ropeGameObject.AddComponent<RopeVisualizer>();
            ropeVisualizer.ropeSpriteShapePrefab = m_RopeCellSpriteShapePrefab;
            ropeVisualizer.VisualizeRope();
        }
    }
}
