using System.Collections.Generic;

namespace Balloondle.Server.Network
{
    public class PlayerManager
    {
        private Dictionary<ulong, Player> players;

        public PlayerManager()
        {
            players = new Dictionary<ulong, Player>();
        }

        public void AddPlayer(Player player)
        {
            players.Add(player.ClientId, player);
        }

        public void RemovePlayerByClientId(ulong clientId) 
        {
            players.Remove(clientId);
        }

        public Player GetPlayerFromClientId(ulong clientId)
        {
            if (!players.ContainsKey(clientId))
            {
                throw new System.ArgumentException($"ClientId '{clientId}' has no player assigned.");
            }

            return players[clientId];
        }
    }
}
