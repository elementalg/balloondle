using System;
using UnityEngine;

namespace Balloondle.Gameplay.World.Effects.Implementations
{
    public class WeaponEffects : MonoBehaviour
    {
        private const string WeaponHit0 = "WeaponHit0";
        
        [SerializeField] 
        private float m_MaxVolume = 0.8f;

        private EffectPlayer _effectPlayer;

        private void OnEnable()
        {
            if (FindObjectOfType<EffectPlayer>() == null)
            {
                throw new InvalidOperationException("Missing EffectPlayer from scene.");
            }
            
            _effectPlayer = FindObjectOfType<EffectPlayer>();
        }

        public void PlayHitEffect(float hitIntensity)
        {
            _effectPlayer.Play(WeaponHit0, gameObject, Vector3.zero, Quaternion.identity, m_MaxVolume * hitIntensity);
        }
    }
}