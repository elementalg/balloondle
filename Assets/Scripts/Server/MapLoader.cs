using Balloondle.Shared.Gameplay;
using MLAPI;
using UnityEngine;

namespace Balloondle.Server
{
    public class MapLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject developmentMapPrefab;

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
