using System;
using UnityEngine;

namespace Balloondle.Gameplay.Physics2D
{
    /// <summary>
    /// Absolute limit indicator of a rope. If exceeded, the rope should break.
    ///
    /// NOTE: Limit is understood as an absolute value, thus negative or positive are treated the same for the limiting
    /// logic.
    /// </summary>
    [Serializable]
    public struct Rope2DArgs
    {
        /// <summary>
        /// Initial length of the rope.
        /// </summary>
        public float m_Length;
        
        /// <summary>
        /// Maximum distance allowed between the connected bodies.
        /// </summary>
        public float m_MaximumDistanceBetweenBodies;
        
        /// <summary>
        /// Maximum force allowed for the joints connecting an end body with a rope cell.
        /// </summary>
        public float m_EndBodiesJointBreakForce;
        
        /// <summary>
        /// Maximum torque allowed for the joints connecting an end body with a rope cell.
        /// </summary>
        public float m_EndBodiesJointBreakTorque;
        
        /// <summary>
        /// Maximum force allowed for the joints connecting two rope cells.
        /// </summary>
        public float m_RopeCellsJointBreakForce;
        
        /// <summary>
        /// Maximum torque allowed for the joints connecting two rope cells. 
        /// </summary>
        public float m_RopeCellsJointBreakTorque;
        
        /// <summary>
        /// Maximum force allowed for the joint connecting the end bodies.
        /// </summary>
        public float m_JointBetweenEndsBreakForce;

        /// <summary>
        /// Maximum velocity supported.
        /// </summary>
        public float m_MaximumSupportedVelocity;

        public Rope2DArgs(float length, float maximumDistanceBetweenBodies, float endBodiesJointBreakForce,
            float endBodiesJointBreakTorque,
            float ropeCellsJointBreakForce, float ropeCellsJointBreakTorque, float jointBetweenEndsBreakForce,
            float maximumSupportedVelocity)
        {
            m_Length = length;
            m_MaximumDistanceBetweenBodies = maximumDistanceBetweenBodies;
            m_EndBodiesJointBreakForce = endBodiesJointBreakForce;
            m_EndBodiesJointBreakTorque = endBodiesJointBreakTorque;
            m_RopeCellsJointBreakForce = ropeCellsJointBreakForce;
            m_RopeCellsJointBreakTorque = ropeCellsJointBreakTorque;
            m_JointBetweenEndsBreakForce = jointBetweenEndsBreakForce;
            m_MaximumSupportedVelocity = maximumSupportedVelocity;
        }
        
        public Rope2DArgs(float maximumDistanceBetweenBodies, float endBodiesJointBreakForce, float endBodiesJointBreakTorque,
            float ropeCellsJointBreakForce, float ropeCellsJointBreakTorque, float jointBetweenEndsBreakForce, float maximumSupportedVelocity)
        {
            m_Length = maximumDistanceBetweenBodies;
            m_MaximumDistanceBetweenBodies = maximumDistanceBetweenBodies;
            m_EndBodiesJointBreakForce = endBodiesJointBreakForce;
            m_EndBodiesJointBreakTorque = endBodiesJointBreakTorque;
            m_RopeCellsJointBreakForce = ropeCellsJointBreakForce;
            m_RopeCellsJointBreakTorque = ropeCellsJointBreakTorque;
            m_JointBetweenEndsBreakForce = jointBetweenEndsBreakForce;
            m_MaximumSupportedVelocity = maximumSupportedVelocity;
        }
    }
}