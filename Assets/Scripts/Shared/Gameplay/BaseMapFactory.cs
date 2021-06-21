using UnityEngine;

namespace Balloondle.Shared.Gameplay
{
    public abstract class BaseMapFactory : MonoBehaviour
    {
        public enum Maps
        {
            DEV,
        }

        [SerializeField]
        protected GameObject developmentMapPrefab;
    }
}