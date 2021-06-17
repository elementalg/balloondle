using System;
using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Essential events which will occur during a match.
    /// </summary>
    public class BaseGamemode : MonoBehaviour
    {
        /// <summary>
        /// Action which is invoked whenever a player has joined the game.
        /// </summary>
        public event Action<ulong> OnPlayerJoin;

        /// <summary>
        /// Actino which is invoked whenever a player has left the game.
        /// </summary>
        public event Action<ulong> OnPlayerQuit;

        /// <summary>
        /// Invoke the OnPlayerJoin action for all the listeners whenever
        /// a client has connected to the server.
        /// </summary>
        /// <param name="clientId">Network Id of the player which has just connected</param>
        public void OnClientJoin(ulong clientId)
        {
            OnPlayerJoin?.Invoke(clientId);
        }

        /// <summary>
        /// Invoke the OnPlayerQuit action for all the listeners whenever
        /// a client has disconnected from the server.
        /// </summary>
        /// <param name="clientId"></param>
        public void OnClientDisconnect(ulong clientId)
        {
            OnPlayerQuit?.Invoke(clientId);
        }

        /// <summary>
        /// Retrieves the current instance of the match.
        /// </summary>
        /// <returns>Current match instance</returns>
        public MatchFunctionality GetCurrentMatch()
        {
            return GameObject.FindWithTag("Match").GetComponent<MatchFunctionality>();
        }
    }
}
