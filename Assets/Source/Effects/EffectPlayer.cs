using UnityEngine;

namespace Balloondle.Effects
{
    public class EffectPlayer : MonoBehaviour
    {
        [SerializeField, Tooltip("Effects which are available to be played through this player.")] 
        private EffectsDefinition m_Effects;

        public void Play(string effectName, GameObject target, Vector3 position, Quaternion rotation)
        {
            foreach (Effect effect in m_Effects.m_Effects)
            {
                if (effect.m_Name.Equals(effectName))
                {
                    Instantiate(effect.m_Prefab, position, rotation, target.transform);
                    
                    return;
                }
            }
            
            Debug.LogWarning($"Could not find effect '{effectName}'");
        }
    }
}