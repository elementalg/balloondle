using UnityEngine;

namespace Balloondle.MiniGame
{
    /// <summary>
    /// Basic score tracker.
    /// </summary>
    public class ScoreManager
    {
        private const string MaxScoreKey = "max-score";
        
        private float _currentScore;
        private float _maxScore;

        public float CurrentScore => _currentScore;
        public float MaxScore => _maxScore;
        
        public ScoreManager()
        {
            _maxScore = PlayerPrefs.GetFloat(MaxScoreKey, 0f);
        }

        public void Score()
        {
            _currentScore += 1f;
        }
    }
}