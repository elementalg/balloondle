using Balloondle.Shared.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Shared
{
    public class MapSpawnLocations : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> spawnLocations;

        private int lastLocation = -1;

        public Vector3 GetSpawnLocation(SpawnMapType spawnMapType, int player)
        {
            // Each map must have the first 6 locations used for spawning
            // the balloons on the two different available teams.
            int index = ((int)spawnMapType) * 3 + player;

            return spawnLocations[index];
        }

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

        public List<Vector3> GetSpawnLocations()
        {
            return spawnLocations;
        }
    }
}
