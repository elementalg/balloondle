using UnityEngine;

namespace Balloondle.Light
{
    public abstract class LightHandler : MonoBehaviour
    {
        /// <summary>
        /// Method called by a <see cref="LightEmitter"/>, when the GameObject's collider containing this component,
        /// has been hit.
        /// </summary>
        /// <param name="hit2D"></param>
        public abstract void OnLightRaycastHit(RaycastHit2D hit2D);
    }
}