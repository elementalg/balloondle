using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Animation
{
    public class UnloadLoadingScene : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SceneManager.UnloadSceneAsync(SceneNames.Loading, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            animator.enabled = false;
        }
    }
}