namespace Balloondle.Server.Gameplay
{
    /// <summary>
    /// Interface which exposes all possible events which may be triggered during the gameplay.
    /// </summary>
    public interface IGamemode : IMatchEventHandler, IMapEventHandler, IPlayerEventHandler
    {
        
    }
}
