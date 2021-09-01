using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Animation
{
    public class SwitchToLoadingScene : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            SceneManager.LoadScene(SceneNames.LOADING, LoadSceneMode.Single);
        }
    }
}