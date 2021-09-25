using System;

namespace Balloondle.MiniGame
{
    [Serializable]
    public struct ObstacleStage
    {
        public float ElapsedTimeRequired;
        public float SpawnEachSeconds;
        public int ObstaclesAmount;
    }
}