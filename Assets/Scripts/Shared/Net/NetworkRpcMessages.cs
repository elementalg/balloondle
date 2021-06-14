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

        public event Action<ulong, ulong> OnPlayerSpawn;

        [ClientRpc]
        public void OnPlayerSpawnClientRpc(ulong targetClientId, ulong playerObjectId)
        {
            OnPlayerSpawn?.Invoke(targetClientId, playerObjectId);
        }

        public event Action<ulong, ulong[]> OnSpawnPlayerBalloonAndWeapon;

        [ClientRpc]
        public void OnSpawnPlayerBalloonAndWeaponClientRpc(ulong ownerId, ulong[] objectIds)
        {
            OnSpawnPlayerBalloonAndWeapon?.Invoke(ownerId, objectIds);
        }

        public event Action<ulong, Vector2> OnPlayerMoveBalloon;

        [ServerRpc]
        public void OnPlayerMoveBalloonServerRpc(ulong playerClientId, Vector2 movement)
        {
            OnPlayerMoveBalloon?.Invoke(playerClientId, movement);
        }

        public event Action<ulong, Vector2> OnPlayerMoveWeapon;

        [ServerRpc]
        public void OnPlayerMoveWeaponServerRpc(ulong playerClientId, Vector2 movement)
        {
            OnPlayerMoveWeapon?.Invoke(playerClientId, movement);
        }
    }
}
