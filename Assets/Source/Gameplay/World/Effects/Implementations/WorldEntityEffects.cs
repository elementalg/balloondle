using System;
using UnityEngine;

namespace Balloondle.Gameplay.World.Effects.Implementations
{
    public class WorldEntityEffects : MonoBehaviour
    {
        private const string Destroy0 = "Destroy0";
        
        [SerializeField] 
        private float m_MaxVolume = 0.8f;

        [SerializeField] private float m_MinVolume = 0.1f;

        private EffectPlayer _effectPlayer;
        private WorldEntity _worldEntity;

        private void OnEnable()
        {
            if (FindObjectOfType<EffectPlayer>() == null)
            {
                throw new InvalidOperationException("Missing EffectPlayer from scene.");
            }
            
            _effectPlayer = FindObjectOfType<EffectPlayer>();

            if (GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("WorldEntity component is missing.");
            }

            _worldEntity = GetComponent<WorldEntity>();
            _worldEntity.OnPreDestroy += PlayDamageEffect;
        }

        private void PlayDamageEffect(float receivedDamage)
        {
            _effectPlayer.Play(Destroy0, null, gameObject.transform.position, Quaternion.identity, 
                Mathf.Max(m_MinVolume, Mathf.Min(m_MaxVolume, receivedDamage / _worldEntity.MaxHealth)));
        }
    }
}