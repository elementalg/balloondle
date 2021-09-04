using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Animation
{
    public class UnloadLoadingScene : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SceneManager.UnloadSceneAsync(SceneNames.LOADING, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            animator.enabled = false;
        }
    }
}