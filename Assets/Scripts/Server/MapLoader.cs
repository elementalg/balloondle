using Balloondle.Shared.Gameplay;
using MLAPI;
using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Loads a specific map by its name.
    /// </summary>
    public class MapLoader : MonoBehaviour
    {
        /// <summary>
        /// Prefab containing the development map.
        /// </summary>
        [SerializeField]
        private GameObject developmentMapPrefab;

        /// <summary>
        /// Load a map by its name.
        /// </summary>
        /// <param name="map">Name of the map.</param>
        public void LoadMap(string map)
        {
            if (map.ToLower().Equals("development"))
            {
                GameObject mapObject = Instantiate(developmentMapPrefab);
                mapObject.GetComponent<NetworkObject>().Spawn();
            }
            else
            {
                throw new System.ArgumentException("Unknown map name.");
            }
        }
    }
}
