namespace Balloondle.Server.Gameplay
{
    public interface IMatchEventHandler
    {
        public void OnMatchStart(Match match);
        public void OnMatchEnd(Match match);
    }
}
