using System;
using UnityEngine;

namespace Balloondle.Server
{
    public class BaseGamemode : MonoBehaviour
    {
        public event Action<ulong> OnPlayerJoin;
        public event Action<ulong> OnPlayerQuit;

        public void OnClientJoin(ulong clientId)
        {
            OnPlayerJoin?.Invoke(clientId);
        }

        public void OnClientDisconnect(ulong clientId)
        {
            OnPlayerQuit?.Invoke(clientId);
        }

        public MatchFunctionality GetCurrentMatch()
        {
            return GameObject.FindWithTag("Match").GetComponent<MatchFunctionality>();
        }
    }
}
