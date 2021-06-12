using Balloondle.Server.Network;

namespace Balloondle.Server.Gameplay
{
    public interface IPlayerEventHandler
    {
        public void OnPlayerJoin(Player player);
        public void OnPlayerQuit(Player player, Player.QuitReason quitReason);
    }
}
