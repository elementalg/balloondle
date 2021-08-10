using System;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class RopeCreator : MonoBehaviour
    {
        private const string RopeTag = "Rope";
        
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
                tag = RopeTag
            };
            
            ropeGameObject.GetComponent<Transform>().position = Vector3.zero;
            Rope2D rope = ropeGameObject.AddComponent<Rope2D>();
            
            // Apply joint break forces/torques.
            rope.endBodiesJointBreakForce = endBodiesJointBreakForce;
            rope.endBodiesJointBreakTorque = endBodiesJointBreakTorque;
            rope.ropeCellsJointBreakForce = ropeCellsJointBreakForce;
            rope.ropeCellsJointBreakTorque = ropeCellsJointBreakTorque;
            
            rope.ropeCellPrefab = m_RopeCellPrefab;
            rope.AddCellsForJoiningStartToEnd(start.gameObject, startAnchor,
                end, endAnchor, maximumDistanceBetweenBodies);

            RopeVisualizer ropeVisualizer = ropeGameObject.AddComponent<RopeVisualizer>();
            ropeVisualizer.ropeSpriteShapePrefab = m_RopeCellSpriteShapePrefab;
            ropeVisualizer.VisualizeRope();
        }
    }
}
