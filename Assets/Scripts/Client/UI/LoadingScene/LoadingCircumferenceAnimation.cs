using UnityEngine;

namespace Balloondle.UI.LoadingScene
{
    public class LoadingCircumferenceAnimation : MonoBehaviour
    {
        private Transform loadingIndicatorState;

        [SerializeField]
        private float zRotationPerFrame;

        /// <summary>
        /// Retrieves the transform component from the parent GameObject.
        /// </summary>
        void Start()
        {
            loadingIndicatorState = gameObject.transform;
        }

        /// <summary>
        /// Rotates the loading indicator.s
        /// </summary>
        void Update()
        {
            loadingIndicatorState.Rotate(0f, 0f, zRotationPerFrame);
        }
    }
}