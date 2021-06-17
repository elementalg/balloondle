using Balloondle.Shared.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Shared
{
    /// <summary>
    /// Standarized way of obtaining spawn locations from a map.
    /// </summary>
    public class MapSpawnLocations : MonoBehaviour
    {
        /// <summary>
        /// List containing all the spawn locations of the map.
        /// </summary>
        [SerializeField]
        private List<Vector3> spawnLocations;

        /// <summary>
        /// Indicator of the last location used for a spawn, in order to avoid
        /// spawning players in the same position.
        /// </summary>
        private int lastLocation = -1;

        /// <summary>
        /// Retrieves a spawn location.
        /// </summary>
        /// <param name="spawnMapType">Type of spawn.</param>
        /// <param name="player">Index for the player.</param>
        /// <returns></returns>
        public Vector3 GetSpawnLocation(SpawnMapType spawnMapType, int player)
        {
            // Each map must have the first 6 locations used for spawning
            // the balloons on the two different available teams.
            int index = ((int)spawnMapType) * 3 + player;

            return spawnLocations[index];
        }

        /// <summary>
        /// Retrieve a random spawn location.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRandomSpawnLocation()
        {
            int randomLocation = Random.Range(0, spawnLocations.Count);

            if (lastLocation == randomLocation)
            {
                return GetRandomSpawnLocation();
            }
            else
            {
                return spawnLocations[randomLocation];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List containing all the spawn locations of the map.</returns>
        public List<Vector3> GetSpawnLocations()
        {
            return spawnLocations;
        }
    }
}
