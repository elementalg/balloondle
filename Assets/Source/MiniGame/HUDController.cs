using Balloondle.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.MiniGame
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField, Tooltip("TextClamp added to the same GameObject as the Text showing the score.")]
        private TextClamp m_ScoreText;
        
        [SerializeField, Tooltip("Image which shows the amount of health through a fill value.")]
        private Image m_HealthValue;
        
        public void UpdateScore(float score)
        {
            m_ScoreText.SetText(score.ToString("0."));
        }

        public void UpdateHealth(float maximumHealth, float health)
        {
            m_HealthValue.fillAmount = health / maximumHealth;
        }
    }
}