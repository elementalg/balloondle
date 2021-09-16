using System;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;

namespace Balloondle.Gameplay
{
    [CreateAssetMenu(fileName = "Rope2DSpawnerConfig", menuName = "Rope2D/Spawner", order = 1)]
    public class Rope2DSpawner : ScriptableObject
    {
        private const string RopeTag = "Rope";
        private const string RopeGameObjectName = "SpawnedRope";
        
        [SerializeField] private GameObject m_Rope2DCellPrefab;
        [SerializeField] private GameObject m_Rope2DCellSpriteShapePrefab;
        [SerializeField, Tooltip("Multiplier applied to the current distance in order to define the maximum distance.")]
        private float m_Rope2DMaximumDistanceMultiplier = 1.25f;
        
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

            limits.MaximumDistanceBetweenBodies = Vector2
                .Distance(start.GetPoint(startAnchor), end.GetPoint(endAnchor)) * m_Rope2DMaximumDistanceMultiplier;
            
            Transform ropeTransform = ropeGameObject.GetComponent<Transform>();
            ropeTransform.position = Vector3.zero;
            ropeTransform.rotation = Quaternion.identity;

            ropeGameObject.AddComponent<WorldEntity>();
            
            Rope2D rope = ropeGameObject.AddComponent<Rope2D>();
            rope.Limits = limits;
            rope.RopeCellPrefab = m_Rope2DCellPrefab;
            rope.AddCellsForJoiningStartToEnd(start.gameObject, startAnchor, end, endAnchor);

            Rope2DVisualizer rope2DVisualizer = ropeGameObject.AddComponent<Rope2DVisualizer>();
            rope2DVisualizer.RopeSpriteShapePrefab = m_Rope2DCellSpriteShapePrefab;
            rope2DVisualizer.VisualizeRope();

            ropeGameObject.AddComponent<Rope2DWorldEntityConfigurator>();
        }
    }
}