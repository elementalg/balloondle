using System;

namespace Balloondle.MiniGame
{
    [Serializable]
    public struct ObstacleStage
    {
        public float m_ElapsedTimeRequired;
        public float m_SpawnEachSeconds;
        public int m_ObstaclesAmount;
    }
}