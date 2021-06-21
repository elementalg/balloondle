using Balloondle.Shared.Gameplay;
using MLAPI;
using MLAPI.Messaging;
using System;
using UnityEngine;

namespace Balloondle.Shared.Network.Game
{
    /// <summary>
    /// Defines all the Rpc messages which may be sent between the server and the client.
    /// </summary>
    public class NetworkRpcMessages : NetworkBehaviour
    {
        /// <summary>
        /// Invoked whenever the server sends the match details to the client.
        /// </summary>
        public event Action<BaseMapFactory.Maps> OnSendMatchDetails;

        [ClientRpc]
        public void OnSendMatchDetailsToClientRpc(BaseMapFactory.Maps map)
        {
            OnSendMatchDetails?.Invoke(map);
        }

        /// <summary>
        /// Invoked whenever the player has been spawned.
        /// </summary>
        public event Action<ulong, ulong> OnPlayerSpawn;

        [ClientRpc]
        public void OnPlayerSpawnClientRpc(ulong targetClientId, ulong playerObjectId)
        {
            OnPlayerSpawn?.Invoke(targetClientId, playerObjectId);
        }

        /// <summary>
        /// Invoked whenever the server has spawned the player's balloon, weapon and thread cells.
        /// </summary>
        public event Action<ulong, ulong[]> OnSpawnPlayerBalloonAndWeapon;

        [ClientRpc]
        public void OnSpawnPlayerBalloonAndWeaponClientRpc(ulong ownerId, ulong[] objectIds)
        {
            OnSpawnPlayerBalloonAndWeapon?.Invoke(ownerId, objectIds);
        }

        /// <summary>
        /// Invoked whenever the match has ended.
        /// </summary>
        public event Action<string> OnMatchEnds;

        [ClientRpc]
        public void OnMatchEndsClientRpc(string matchStats)
        {
            OnMatchEnds?.Invoke(matchStats);
        }

        /// <summary>
        /// Invoked whenever the player has decided to move its balloon.
        /// </summary>
        public event Action<ulong, Vector2> OnPlayerMoveBalloon;

        [ServerRpc(Delivery = RpcDelivery.Unreliable, RequireOwnership = false)]
        public void OnPlayerMoveBalloonServerRpc(ulong playerClientId, Vector2 movement)
        {
            OnPlayerMoveBalloon?.Invoke(playerClientId, movement);
        }

        /// <summary>
        /// Invoked whenever the player has decided to move its weapon.
        /// </summary>
        public event Action<ulong, Vector2> OnPlayerMoveWeapon;

        [ServerRpc(Delivery = RpcDelivery.Unreliable, RequireOwnership = false)]
        public void OnPlayerMoveWeaponServerRpc(ulong playerClientId, Vector2 movement)
        {
            OnPlayerMoveWeapon?.Invoke(playerClientId, movement);
        }
    }
}
