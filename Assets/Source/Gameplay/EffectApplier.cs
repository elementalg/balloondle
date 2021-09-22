using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class EffectApplier : MonoBehaviour
    {
        [SerializeField, Tooltip("Effect's prefab to be applied.")] 
        private GameObject m_EffectPrefab;

        [SerializeField, Tooltip("Effect's local space position.")]
        private Vector3 m_EffectOffset = Vector3.zero;
        
        [SerializeField, Tooltip("Effect's local space rotation.")]
        private Vector3 m_EffectOffsetRotation = Vector3.zero;

        private void OnEnable()
        {
            if (m_EffectPrefab == null)
            {
                throw new InvalidOperationException("EffectApplier requires an effect prefab.");
            }
        }

        public void ApplyEffect()
        {
            GameObject effect = Instantiate(m_EffectPrefab, transform);
            effect.transform.localPosition = m_EffectOffset;
            effect.transform.localRotation = Quaternion.Euler(m_EffectOffsetRotation);
        }
    }
}
