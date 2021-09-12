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

        public void CreateRopeConnectingTwoRigidBodies2D(Rigidbody2D start, Vector2 startAnchor, 
            Rigidbody2D end, Vector2 endAnchor, float maximumDistanceBetweenBodies)
        {
            CreateRopeConnectingTwoRigidBodies2D(start, startAnchor, end, endAnchor, maximumDistanceBetweenBodies,
                m_DefaultEndPointJointBreakForce, m_DefaultEndPointJointBreakTorque,
                m_DefaultRopeCellJointBreakForce, m_DefaultRopeCellJointBreakTorque);
        }

        public void CreateRopeConnectingTwoRigidBodies2D(Rigidbody2D start, Vector2 startAnchor, Rigidbody2D end,
            Vector2 endAnchor, float maximumDistanceBetweenBodies,
            float endBodiesJointBreakForce, float endBodiesJointBreakTorque,
            float ropeCellsJointBreakForce, float ropeCellsJointBreakTorque)
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
            rope.EndBodiesJointBreakForce = endBodiesJointBreakForce;
            rope.EndBodiesJointBreakTorque = endBodiesJointBreakTorque;
            rope.RopeCellsJointBreakForce = ropeCellsJointBreakForce;
            rope.RopeCellsJointBreakTorque = ropeCellsJointBreakTorque;
            
            rope.RopeCellPrefab = m_RopeCellPrefab;
            rope.AddCellsForJoiningStartToEnd(start.gameObject, startAnchor,
                end, endAnchor, maximumDistanceBetweenBodies);

            RopeVisualizer ropeVisualizer = ropeGameObject.AddComponent<RopeVisualizer>();
            ropeVisualizer.RopeSpriteShapePrefab = m_RopeCellSpriteShapePrefab;
            ropeVisualizer.VisualizeRope();
        }
    }
}
