using System;
using UnityEngine;

namespace Balloondle
{
    public class AnimationSoundEffectPlayer : StateMachineBehaviour
    {
        [SerializeField, Tooltip("Delay, in seconds, applied to the playing of the sound effect.")]
        private float m_SoundEffectDelay = 0.1f;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetComponent<AudioSource>() == null)
            {
                throw new InvalidOperationException(
                    "The GameObject with the AnimationController must contain an AudioSource.");
            }
            
            animator.GetComponent<AudioSource>().PlayDelayed(m_SoundEffectDelay);
        }
    }
}
