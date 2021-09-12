using UnityEngine;

namespace Balloondle.UI.LoadingScene
{
    /// <summary>
    /// Animate the loading circumference by making it spin.
    /// </summary>
    public class LoadingCircumferenceAnimation : MonoBehaviour
    {
        private Transform _loadingIndicatorState;

        /// <summary>
        /// Amount of degrees which are added on each frame.
        /// </summary>
        [SerializeField]
        private float m_ZRotationPerFrame;

        /// <summary>
        /// Retrieves the transform component from the parent GameObject.
        /// </summary>
        private void Start()
        {
            _loadingIndicatorState = gameObject.transform;
        }

        /// <summary>
        /// Rotates the loading indicator.s
        /// </summary>
        private void Update()
        {
            _loadingIndicatorState.Rotate(0f, 0f, m_ZRotationPerFrame);
        }
    }
}