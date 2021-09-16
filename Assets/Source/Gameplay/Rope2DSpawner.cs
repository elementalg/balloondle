using System;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class Rope2DSpawner : MonoBehaviour
    {
        private const string RopeTag = "Rope";
        private const string RopeGameObjectName = "SpawnedRope";
        
        [SerializeField] private GameObject m_RopeCellPrefab;
        [SerializeField] private GameObject m_RopeCellSpriteShapePrefab;
        
        public void CreateRopeConnectingTwoRigidBodies2D(Rigidbody2D start, Vector2 startAnchor, Rigidbody2D end,
            Vector2 endAnchor, Rope2DLimits limits)
        {
            if (start == end)
            {
                throw new InvalidOperationException("Cannot create a rope connecting the same rigid body.");
            }

            GameObject ropeGameObject = new GameObject
            {
                tag = RopeTag,
                name = RopeGameObjectName
            };

            Transform ropeTransform = GetComponent<Transform>();
            ropeTransform.position = Vector3.zero;
            ropeTransform.rotation = Quaternion.identity;

            ropeGameObject.AddComponent<WorldEntity>();
            
            Rope2D rope = ropeGameObject.AddComponent<Rope2D>();
            rope.Limits = limits;
            rope.RopeCellPrefab = m_RopeCellPrefab;
            rope.AddCellsForJoiningStartToEnd(start.gameObject, startAnchor, end, endAnchor);

            Rope2DVisualizer rope2DVisualizer = ropeGameObject.AddComponent<Rope2DVisualizer>();
            rope2DVisualizer.RopeSpriteShapePrefab = m_RopeCellSpriteShapePrefab;
            rope2DVisualizer.VisualizeRope();

            ropeGameObject.AddComponent<Rope2DWorldEntityConfigurator>();
        }
    }
}