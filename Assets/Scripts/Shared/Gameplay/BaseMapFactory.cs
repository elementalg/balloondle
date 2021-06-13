using UnityEngine;

namespace Balloondle.Shared.Gameplay
{
    public abstract class BaseMapFactory : MonoBehaviour
    {
        public enum Maps
        {
            DEVELOPMENT,
        }

        [SerializeField]
        protected GameObject developmentMapPrefab;
    }
}