using UnityEngine;

namespace Balloondle.UI.LoadingScene
{
    /// <summary>
    /// Animate the loading circumference by making it spin.
    /// </summary>
    public class LoadingCircumferenceAnimation : MonoBehaviour
    {
        private Transform loadingIndicatorState;

        /// <summary>
        /// Amount of degrees which are added on each frame.
        /// </summary>
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