using UnityEngine;

namespace Balloondle.Server.Gameplay.Gamemodes
{
    public class Test : IGamemode
    {
        internal Test()
        {

        }

        public void OnMatchEnd(Match match)
        {
            Debug.Log("OnMatchEnd");
        }

        public void OnMatchStart(Match match)
        {
            Debug.Log("OnMatchStart");
        }

        public void OnMapStart(Map map)
        {
            Debug.Log("OnMapStart");
        }

        public void OnMapRestart(Map map)
        {
            Debug.Log("OnMapRestart");
        }

        public void OnMapStop(Map map)
        {
            Debug.Log("OnMapStop");
        }
    }
}
