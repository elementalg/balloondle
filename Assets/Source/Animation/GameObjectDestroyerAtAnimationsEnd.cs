using UnityEngine;

namespace Balloondle.Animation
{
    public class GameObjectDestroyerAtAnimationsEnd : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }
    }
}