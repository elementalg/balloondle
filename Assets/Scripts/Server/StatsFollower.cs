using Balloondle.Shared.Net.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Server
{
    public class StatsFollower : MonoBehaviour
    {
        public Dictionary<ulong, PlayerMatchStats> PlayerStats { get; } = 
            new Dictionary<ulong, PlayerMatchStats>();

        /// <summary>
        /// Listen to incoming players.
        /// </summary>
        void Start()
        {
            GetComponent<BaseGamemode>().OnPlayerJoin += OnPlayerJoin;
            GetComponent<BaseGamemode>().OnPlayerQuit += OnPlayerQuit;
        }

        private void OnPlayerJoin(ulong playerClientId) 
        {
            PlayerMatchStats playerMatchStats = new PlayerMatchStats();
            playerMatchStats.damage = 0f;
            playerMatchStats.position = (uint)PlayerStats.Count;

            PlayerStats.Add(playerClientId, playerMatchStats);
        }

        private void OnPlayerQuit(ulong playerClientId)
        {
            if (PlayerStats.ContainsKey(playerClientId))
            {
                PlayerStats.Remove(playerClientId);
            }
        }

        public void OnPlayerDoesDamage(ulong playerClientId, float damageAmount)
        {
            PlayerStats[playerClientId].damage += damageAmount;
        }
       
        /// <summary>
        /// Proceed to calculate the positions of the players in the leaderboard.
        /// </summary>
        public void CalculatePlayersLeaderboardPosition()
        {
            ulong[] playerPositions = new ulong[PlayerStats.Count];
            int count = 0;
                
            foreach (KeyValuePair<ulong, PlayerMatchStats> entry in PlayerStats)
            {
                // Assign the player's client id to the position they are in
                playerPositions[count] = entry.Key;

                count++;
            }

            for (int i = 0; i < playerPositions.Length; i++)
            {
                ulong playerPosition = playerPositions[i];

                for (int j = playerPositions.Length - 1; j >= 0; j--)
                {
                    ulong otherPlayerPosition = playerPositions[j];

                    if (PlayerStats[playerPosition].damage < PlayerStats[otherPlayerPosition].damage)
                    {
                        playerPositions[i] = otherPlayerPosition;
                        playerPositions[j] = playerPosition;
                    }
                }
            }

            for (uint i = 0; i < playerPositions.Length; i++)
            {
                PlayerStats[playerPositions[i]].position = i;
            }
        }
    }
}