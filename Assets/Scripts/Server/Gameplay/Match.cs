namespace Balloondle.Server.Gameplay
{
    public class Match
    {
        public Gamemode Mode;
        public Map GameMap;

        public Match(Gamemode gamemode, Map gameMap)
        {
            Mode = gamemode;
            GameMap = gameMap;
        }

        public void Start()
        {
            GameMap.Start(Mode);

            Mode.OnMatchStart(this);
        }

        public void End()
        {
            Mode.OnMatchEnd(this);

            GameMap.Stop(Mode);
        }
    }
}
