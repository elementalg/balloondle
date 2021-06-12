using Balloondle.Server.Gameplay.Gamemodes;

namespace Balloondle.Server.Gameplay
{
    public class GamemodeFactory
    {
        public enum Gamemodes
        {
            TEST,
        }

        public IGamemode BuildGamemode(Gamemodes gamemode)
        {
            switch (gamemode)
            {
                case Gamemodes.TEST:
                    return new Test();
                default:
                    throw new System.ArgumentException("Unknown gamemode detected.", nameof(gamemode));
            }
        }
    }
}