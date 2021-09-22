using UnityEngine;

namespace Balloondle.Effects
{
    public class EffectPlayer : MonoBehaviour
    {
        [SerializeField, Tooltip("Effects which are available to be played through this player.")] 
        private EffectsDefinition m_Effects;

        public void Play(string effectName, GameObject target, Vector3 position, Quaternion rotation, float volume = 1f)
        {
            foreach (Effect effect in m_Effects.m_Effects)
            {
                if (effect.m_Name.Equals(effectName))
                {
                    GameObject effectObject;
                    
                    if (target != null)
                    {
                        effectObject = Instantiate(effect.m_Prefab, position, rotation, target.transform);
                    }
                    else
                    {
                        effectObject = Instantiate(effect.m_Prefab, position, rotation);
                    }
                    

                    if (effectObject.GetComponent<AudioSource>())
                    {
                        effectObject.GetComponent<AudioSource>().volume = volume;
                    }
                    
                    return;
                }
            }
            
            Debug.LogWarning($"Could not find effect '{effectName}'");
        }
    }
}