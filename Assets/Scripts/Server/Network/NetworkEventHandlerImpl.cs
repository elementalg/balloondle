using Balloondle.Server.Gameplay;
using Balloondle.Shared.Game;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;

namespace Balloondle.Server.Network
{
    public class NetworkEventHandlerImpl : INetworkEventHandler
    {
        private PlayerManager playerManager;

        public Match CurrentMatch { get; set; }

        public NetworkEventHandlerImpl()
        {
            playerManager = new PlayerManager();
        }

        public void OnConnectionApprovalRequest(byte[] connectionData, ulong clientId,
            MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
        {
            if (connectionData.Length == 0)
            {
                Debug.Log("Declining incoming connection due to lack of connection data.");

                // If no connection data, decline the connection.
                callback(false, null, false, null, null);
                return;
            }

            string uncheckedPlayerData = Encoding.UTF8.GetString(connectionData);

            // TODO: Sanity check 'uncheckedPlayerData'.

            PlayerConnectionData playerConnectionData = JsonConvert
                .DeserializeObject<PlayerConnectionData>(uncheckedPlayerData);

            // TODO: Validate 'playerConnectionData'.


            Player player = new Player(playerConnectionData.Name, clientId);

            playerManager.AddPlayer(player);

            Debug.Log($"Accepted connection for player with name '{player.Name}'.");
        }

        public void OnClientConnected(ulong clientId)
        {
            // TODO: Assure CurrentMatch is set before any client gets connected.

            if (CurrentMatch != null)
            {
                Player player = playerManager.GetPlayerFromClientId(clientId);
                CurrentMatch.Mode.OnPlayerJoin(player);
            }
        }

        public void OnClientDisconnected(ulong clientId)
        {
            if (CurrentMatch != null)
            {
                Player player = playerManager.GetPlayerFromClientId(clientId);
                CurrentMatch.Mode.OnPlayerQuit(player, Player.QuitReason.DISCONNECT);
            }
        }
    }
}
