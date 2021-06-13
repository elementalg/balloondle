using Balloondle.Shared.Gameplay;
using MLAPI;
using UnityEngine;

namespace Balloondle.Server
{
    public class MapLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject developmentMapPrefab;

        public void LoadMap(BaseMapFactory.Maps map)
        {
            switch (map)
            {
                case BaseMapFactory.Maps.DEVELOPMENT:
                    GameObject mapObject = Instantiate(developmentMapPrefab);
                    mapObject.GetComponent<NetworkObject>().Spawn();

                    break;
                default:
                    break;
            }
        }
    }
}
