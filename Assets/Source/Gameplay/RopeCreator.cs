using System;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class RopeCreator : MonoBehaviour
    {
        private const string RopeTag = "Rope";
        private const string RopeGameObjectName = "GeneratedRope";
        
        [SerializeField] private GameObject m_RopeCellPrefab;
        [SerializeField] private GameObject m_RopeCellSpriteShapePrefab;
        
        [SerializeField] private float m_DefaultEndPointJointBreakForce = 10f;
        [SerializeField] private float m_DefaultEndPointJointBreakTorque = 10f;
        [SerializeField] private float m_DefaultRopeCellJointBreakForce = 2f;
        [SerializeField] private float m_DefaultRopeCellJointBreakTorque = 2f;
        [SerializeField] private float m_DefaultJointBetweenEndsBreakForce = 1000f;

        public void CreateRopeConnectingTwoRigidBodies2D(Rigidbody2D start, Vector2 startAnchor, 
            Rigidbody2D end, Vector2 endAnchor, float maximumDistanceBetweenBodies)
        {
            Rope2DLimits defaultLimits = new Rope2DLimits(maximumDistanceBetweenBodies,
                m_DefaultEndPointJointBreakForce, m_DefaultEndPointJointBreakTorque,
                m_DefaultRopeCellJointBreakForce, m_DefaultRopeCellJointBreakTorque,
                m_DefaultJointBetweenEndsBreakForce);
            
            CreateRopeConnectingTwoRigidBodies2D(start, startAnchor, end, endAnchor, defaultLimits);
        }

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
            
            ropeGameObject.GetComponent<Transform>().position = Vector3.zero;
            Rope2D rope = ropeGameObject.AddComponent<Rope2D>();
            
            // Apply joint break forces/torques.
            rope.EndBodiesJointBreakForce = limits.EndBodiesJointBreakForce;
            rope.EndBodiesJointBreakTorque = limits.EndBodiesJointBreakTorque;
            rope.RopeCellsJointBreakForce = limits.RopeCellsJointBreakForce;
            rope.RopeCellsJointBreakTorque = limits.RopeCellsJointBreakTorque;
            rope.JointBetweenEndsBreakForce = limits.JointBetweenEndsBreakForce;
            
            rope.RopeCellPrefab = m_RopeCellPrefab;
            rope.AddCellsForJoiningStartToEnd(start.gameObject, startAnchor,
                end, endAnchor, limits.MaximumDistanceBetweenBodies);

            RopeVisualizer ropeVisualizer = ropeGameObject.AddComponent<RopeVisualizer>();
            ropeVisualizer.RopeSpriteShapePrefab = m_RopeCellSpriteShapePrefab;
            ropeVisualizer.VisualizeRope();
        }
    }
}
