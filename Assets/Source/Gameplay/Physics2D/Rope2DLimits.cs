namespace Balloondle.Gameplay.Physics2D
{
    public struct Rope2DLimits
    {
        public float MaximumDistanceBetweenBodies;
        public float EndBodiesJointBreakForce;
        public float EndBodiesJointBreakTorque;
        public float RopeCellsJointBreakForce;
        public float RopeCellsJointBreakTorque;
        public float JointBetweenEndsBreakForce;

        public Rope2DLimits(float maximumDistanceBetweenBodies, float endBodiesJointBreakForce, float endBodiesJointBreakTorque,
            float ropeCellsJointBreakForce, float ropeCellsJointBreakTorque, float jointBetweenEndsBreakForce)
        {
            MaximumDistanceBetweenBodies = maximumDistanceBetweenBodies;
            EndBodiesJointBreakForce = endBodiesJointBreakForce;
            EndBodiesJointBreakTorque = endBodiesJointBreakTorque;
            RopeCellsJointBreakForce = ropeCellsJointBreakForce;
            RopeCellsJointBreakTorque = ropeCellsJointBreakTorque;
            JointBetweenEndsBreakForce = jointBetweenEndsBreakForce;
        }
    }
}