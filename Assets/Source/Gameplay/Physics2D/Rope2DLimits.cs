using UnityEngine;

namespace Balloondle.Gameplay.Physics2D
{
    /// <summary>
    /// Absolute limit indicator of a rope. If exceeded, the rope should break.
    ///
    /// NOTE: Limit is understood as an absolute value, thus negative or positive are treated the same for the limiting
    /// logic.
    /// </summary>
    public struct Rope2DLimits
    {
        /// <summary>
        /// Maximum distance allowed between the connected bodies.
        /// </summary>
        public float MaximumDistanceBetweenBodies;
        
        /// <summary>
        /// Maximum force allowed for the joints connecting an end body with a rope cell.
        /// </summary>
        public float EndBodiesJointBreakForce;
        
        /// <summary>
        /// Maximum torque allowed for the joints connecting an end body with a rope cell.
        /// </summary>
        public float EndBodiesJointBreakTorque;
        
        /// <summary>
        /// Maximum force allowed for the joints connecting two rope cells.
        /// </summary>
        public float RopeCellsJointBreakForce;
        
        /// <summary>
        /// Maximum torque allowed for the joints connecting two rope cells. 
        /// </summary>
        public float RopeCellsJointBreakTorque;
        
        /// <summary>
        /// Maximum force allowed for the joint connecting the end bodies.
        /// </summary>
        public float JointBetweenEndsBreakForce;

        /// <summary>
        /// Maximum velocity supported.
        /// </summary>
        public float MaximumSupportedVelocity;

        public Rope2DLimits(float maximumDistanceBetweenBodies, float endBodiesJointBreakForce, float endBodiesJointBreakTorque,
            float ropeCellsJointBreakForce, float ropeCellsJointBreakTorque, float jointBetweenEndsBreakForce, float maximumSupportedVelocity)
        {
            MaximumDistanceBetweenBodies = maximumDistanceBetweenBodies;
            EndBodiesJointBreakForce = endBodiesJointBreakForce;
            EndBodiesJointBreakTorque = endBodiesJointBreakTorque;
            RopeCellsJointBreakForce = ropeCellsJointBreakForce;
            RopeCellsJointBreakTorque = ropeCellsJointBreakTorque;
            JointBetweenEndsBreakForce = jointBetweenEndsBreakForce;
            MaximumSupportedVelocity = maximumSupportedVelocity;
        }
    }
}