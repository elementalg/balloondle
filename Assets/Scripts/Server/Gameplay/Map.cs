using UnityEngine;

namespace Balloondle.Server.Gameplay
{
    public class Map
    {
        private GameObject mapPrefab;

        internal Map(GameObject mapPrefab)
        {
            this.mapPrefab = mapPrefab;
        }

        public void Start(IGamemode gamemode)
        {
            gamemode.OnMapStart(this);
        }

        public void Restart(IGamemode gamemode)
        {
            gamemode.OnMapRestart(this);
        }

        public void Stop(IGamemode gamemode)
        {
            gamemode.OnMapStop(this);
        }
    }
}