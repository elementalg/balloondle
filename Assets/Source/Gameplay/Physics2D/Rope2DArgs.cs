﻿using System;
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
        public float Length;
        
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

        public Rope2DArgs(float length, float maximumDistanceBetweenBodies, float endBodiesJointBreakForce,
            float endBodiesJointBreakTorque,
            float ropeCellsJointBreakForce, float ropeCellsJointBreakTorque, float jointBetweenEndsBreakForce,
            float maximumSupportedVelocity)
        {
            Length = length;
            MaximumDistanceBetweenBodies = maximumDistanceBetweenBodies;
            EndBodiesJointBreakForce = endBodiesJointBreakForce;
            EndBodiesJointBreakTorque = endBodiesJointBreakTorque;
            RopeCellsJointBreakForce = ropeCellsJointBreakForce;
            RopeCellsJointBreakTorque = ropeCellsJointBreakTorque;
            JointBetweenEndsBreakForce = jointBetweenEndsBreakForce;
            MaximumSupportedVelocity = maximumSupportedVelocity;
        }
        
        public Rope2DArgs(float maximumDistanceBetweenBodies, float endBodiesJointBreakForce, float endBodiesJointBreakTorque,
            float ropeCellsJointBreakForce, float ropeCellsJointBreakTorque, float jointBetweenEndsBreakForce, float maximumSupportedVelocity)
        {
            Length = maximumDistanceBetweenBodies;
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