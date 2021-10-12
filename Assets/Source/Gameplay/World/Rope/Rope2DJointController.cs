using System;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;

namespace Balloondle.Gameplay.World.Rope
{
    public class Rope2DJointController : MonoBehaviour
    {
        private HingeJoint2D _hingeJoint2D;
        private DistanceJoint2D _distanceJoint2D;

        public Rope2D RopeInstance { get; set; }
        
        private void Start()
        {
            if (GetComponent<HingeJoint2D>() == null)
            {
                throw new InvalidOperationException(
                    "RopeJointController requires the GameObject to contain a HingeJoint2D component.");
            }

            if (GetComponent<DistanceJoint2D>() == null)
            {
                throw new InvalidOperationException(
                    "RopeJointController requires the GameObject to contain a DistanceJoint2D component.");
            }

            _hingeJoint2D = GetComponent<HingeJoint2D>();
            _distanceJoint2D = GetComponent<DistanceJoint2D>();
        }

        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            if (brokenJoint == _hingeJoint2D)
            {
                _distanceJoint2D.enabled = false;
            }
            else
            {
                _hingeJoint2D.enabled = false;
            }

            if (RopeInstance != null)
            {
                RopeInstance.Break();
            }
        }
    }
}