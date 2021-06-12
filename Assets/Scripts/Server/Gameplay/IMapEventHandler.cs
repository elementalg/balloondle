namespace Balloondle.Server.Gameplay
{
    public interface IMapEventHandler
    {
        public void OnMapStart(Map map);
        public void OnMapRestart(Map map);
        public void OnMapStop(Map map);
    }
}
