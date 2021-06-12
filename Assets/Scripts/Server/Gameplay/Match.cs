namespace Balloondle.Server.Gameplay
{
    public class Match
    {
        public enum State
        {
            NOT_STARTED,
            STARTED,
            ENDED,
        }

        public IGamemode Mode { get; private set; }
        public Map GameMap { get; private set; }
        public State MatchState { get; private set; }

        public Match(IGamemode gamemode, Map gameMap)
        {
            Mode = gamemode;
            GameMap = gameMap;
            MatchState = State.NOT_STARTED;
        }

        public void Start()
        {
            Mode.OnMatchStart(this);

            MatchState = State.STARTED;

            GameMap.Start(Mode);
        }

        public void End()
        {
            GameMap.Stop(Mode);

            MatchState = State.ENDED;

            Mode.OnMatchEnd(this);
        }
    }
}