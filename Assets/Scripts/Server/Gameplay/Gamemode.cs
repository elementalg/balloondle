namespace Balloondle.Server.Gameplay
{
    /// <summary>
    /// Interface which exposes all possible events which may be triggered during the gameplay.
    /// </summary>
    public interface Gamemode
    {
        public void OnMatchStart(Match match);
        public void OnMatchEnd(Match match);

        public void OnMapStart(Map map);
        public void OnMapRestart(Map map);
        public void OnMapStop(Map map);
    }
}
