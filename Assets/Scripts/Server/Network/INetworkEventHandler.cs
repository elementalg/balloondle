namespace Balloondle.Server.Network
{
    public interface INetworkEventHandler
    {
        public void OnConnectionApprovalRequest(byte[] connectionData, ulong clientId,
            MLAPI.NetworkManager.ConnectionApprovedDelegate callback);

        public void OnClientConnected(ulong clientId);
        public void OnClientDisconnected(ulong clientId);
    }
}
