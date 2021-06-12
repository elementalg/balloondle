namespace Balloondle.Server.Network
{
    public class Player
    {
        public enum QuitReason
        {
            DISCONNECT,
            TIMEOUT,
            KICKED,
        }

        public string Name { get; private set; }
        public ulong ClientId { get; private set; }

        public Player(string name, ulong clientId)
        {
            Name = name;
            ClientId = clientId;
        }
    }
}
