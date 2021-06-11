using System;
using UnityEngine;

namespace Balloondle.Server.Gameplay
{
    public class MapFactory : MonoBehaviour
    {
        public enum Maps
        {
            DEVELOPMENT,
        }

        [SerializeField]
        private GameObject developmentMapPrefab;

        public Map BuildMap(Maps map)
        {
            switch (map)
            {
                case Maps.DEVELOPMENT:
                    return new Map(developmentMapPrefab);
                default:
                    throw new System.ArgumentException("Invalid map value.", nameof(map));
            }
        }
    }
}
