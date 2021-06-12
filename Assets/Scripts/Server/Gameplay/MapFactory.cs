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
            return map switch
            {
                Maps.DEVELOPMENT => new Map(developmentMapPrefab),
                _ => throw new System.ArgumentException("Invalid map value.", nameof(map)),
            };
        }
    }
}