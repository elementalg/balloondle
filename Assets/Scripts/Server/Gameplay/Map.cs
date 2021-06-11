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

        public void Start(Gamemode gamemode)
        {
            gamemode.OnMapStart(this);
        }

        public void Restart(Gamemode gamemode)
        {
            gamemode.OnMapRestart(this);
        }

        public void Stop(Gamemode gamemode)
        {
            gamemode.OnMapStop(this);
        }
    }
}
