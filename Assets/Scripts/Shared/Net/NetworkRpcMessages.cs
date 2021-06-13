using Balloondle.Shared.Gameplay;
using MLAPI;
using MLAPI.Messaging;
using System;
using UnityEngine;

namespace Balloondle.Shared.Network.Game
{
    public class NetworkRpcMessages : NetworkBehaviour
    {
        public event Action<BaseMapFactory.Maps> OnSendMatchDetails;

        [ClientRpc]
        public void OnSendMatchDetailsToClientRpc(BaseMapFactory.Maps map)
        {
            OnSendMatchDetails?.Invoke(map);
        }

        public event Action<ulong> OnPlayerSpawn;

        [ClientRpc]
        public void OnPlayerSpawnClientRpc(ulong playerObjectId)
        {
            OnPlayerSpawn?.Invoke(playerObjectId);
        }
    }
}
