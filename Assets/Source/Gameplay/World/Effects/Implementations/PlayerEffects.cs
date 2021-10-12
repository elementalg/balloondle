using System;
using UnityEngine;

namespace Balloondle.Gameplay.World.Effects.Implementations
{
    public class PlayerEffects : MonoBehaviour
    {
        private const string BalloonConfusion = "BalloonConfusion";
        private const string BalloonCollision = "BalloonCollision";
        
        [SerializeField] 
        private float m_MinSquaredVelocityMagnitudeForSound = 25f;

        [SerializeField] 
        private float m_MaxSoundSquaredVelocityMagnitudeForSound = 200f;

        [SerializeField] 
        private float m_MaxVolume = 0.8f;

        [SerializeField] 
        private float m_ConfusionRelativeVolume = 0.5f;
        
        private Player _player;
        private EffectPlayer _effectPlayer;

        private void OnEnable()
        {
            if (GetComponent<Player>() == null)
            {
                throw new InvalidOperationException("Missing Player component.");
            }
            
            _player = GetComponent<Player>();
            
            if (FindObjectOfType<EffectPlayer>() == null)
            {
                throw new InvalidOperationException("Missing EffectPlayer from scene.");
            }
            
            _effectPlayer = FindObjectOfType<EffectPlayer>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.relativeVelocity.sqrMagnitude >= m_MinSquaredVelocityMagnitudeForSound)
            {
                // Don't play sound when player hits itself with the weapon.
                if (_player.HasWeapon() && _player.GetWeapon().gameObject == other.gameObject)
                {
                    return;
                }
                
                _effectPlayer.Play(BalloonCollision, gameObject, Vector3.zero, Quaternion.identity,
                    Mathf.Min(m_MaxVolume, other.relativeVelocity.sqrMagnitude / m_MaxSoundSquaredVelocityMagnitudeForSound));
            }
        }

        public void Confusion()
        {
            _effectPlayer.Play(BalloonConfusion, gameObject, new Vector3(0f, 0.7f, 0f), Quaternion.identity,
                m_ConfusionRelativeVolume * m_MaxVolume);
        }
    }
}