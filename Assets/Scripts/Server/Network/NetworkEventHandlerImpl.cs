using Balloondle.Server.Gameplay;
using Balloondle.Shared.Game;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;

namespace Balloondle.Server.Network
{
    public class NetworkEventHandlerImpl : INetworkEventHandler
    {
        private Match match;
        private string serverPassword;
        private PlayerManager playerManager;

        public Match CurrentMatch { get; set; }

        public NetworkEventHandlerImpl(Match match, string serverPassword)
        {
            this.match = match;
            this.serverPassword = serverPassword;

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

            if (!IsPlayerConnectionDataValid(playerConnectionData))
            {
                callback(false, null, false, null, null);
                return;
            }

            Player player = new Player(playerConnectionData.Name, clientId);

            playerManager.AddPlayer(player);

            Debug.Log($"Accepted connection for player with name '{player.Name}'.");

            callback(false, null, true, null, null);
        }

        private bool IsPlayerConnectionDataValid(PlayerConnectionData playerConnectionData)
        {
            // TODO: Validate 'playerConnectionData'.

            if (!playerConnectionData.ServerPassword.Equals(serverPassword))
            {
                return false;
            }

            return true;
        }

        public void OnClientConnected(ulong clientId)
        {
            // TODO: Assure CurrentMatch is set before any client gets connected.
            Debug.Log($"OnClientConnected {clientId}");

            if (CurrentMatch != null)
            {
                Player player = playerManager.GetPlayerFromClientId(clientId);
                CurrentMatch.Mode.OnPlayerJoin(player);
            }
        }

        public void OnClientDisconnected(ulong clientId)
        {
            Debug.Log($"OnClientDisconnected {clientId}");

            if (CurrentMatch != null)
            {
                Player player = playerManager.GetPlayerFromClientId(clientId);
                CurrentMatch.Mode.OnPlayerQuit(player, Player.QuitReason.DISCONNECT);
            }
        }
    }
}
