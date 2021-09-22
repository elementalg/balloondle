using System;
using Balloondle.Gameplay.World;
using UnityEngine;

namespace Balloondle.Effects.Implementations
{
    public class PlayerEffects : MonoBehaviour
    {
        private const string BalloonCollision = "BalloonCollision";
        
        [SerializeField] 
        private float m_MinSquaredVelocityMagnitudeForSound = 25f;

        [SerializeField] 
        private float m_MaxSoundSquaredVelocityMagnitudeForSound = 200f;

        [SerializeField] 
        private float m_MaxVolume = 0.8f;

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
    }
}