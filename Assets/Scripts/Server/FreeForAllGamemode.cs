using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// WIP gamemode implementation.
    /// </summary>
    public class FreeForAllGamemode : MonoBehaviour
    {
        /// <summary>
        /// Amount of minimum required players before spawning the players and their
        /// objects.
        /// </summary>
        [SerializeField]
        private int minimumRequiredPlayers = 1;

        /// <summary>
        /// Amount of seconds which the match lasts on this gamemode.
        /// </summary>
        [SerializeField]
        private float matchDurationInSeconds = 60f;

        /// <summary>
        /// Amount of seconds which have elapsed since the start of the match.
        /// </summary>
        private float elapsedTime = 0f;

        /// <summary>
        /// Count of the amount of players playing.
        /// </summary>
        private int playersCount = 0;

        /// <summary>
        /// Handle the connections and disconnections of the players.
        /// </summary>
        void Start()
        {
            GetComponent<BaseGamemode>().OnPlayerJoin += OnPlayerJoin;
            GetComponent<BaseGamemode>().OnPlayerQuit += OnPlayerQuit;
        }

        /// <summary>
        /// Handle the start of the player joins.
        /// </summary>
        /// <param name="clientId"></param>
        void OnPlayerJoin(ulong clientId)
        {
            Debug.Log("OnPlayerJoin");

            ++playersCount;

            MatchFunctionality match = GetComponent<BaseGamemode>().GetCurrentMatch();

            Debug.Log($"MatchState: {match.State}");

            // If the match is on pause, check if the minimum required players has been
            // accomplished.
            switch (match.State)
            {
                case MatchFunctionality.MatchState.PAUSE:
                    if (playersCount >= minimumRequiredPlayers)
                    {
                        GetComponent<PlayerManager>().SpawnPlayers();
                        match.State = MatchFunctionality.MatchState.RUNNING;
                    }

                    break;
                case MatchFunctionality.MatchState.RUNNING:
                    GetComponent<PlayerManager>().SpawnPlayer(clientId);
                    break;
                default:
                    // TODO: Disconnect
                    break;
            }
        }

        /// <summary>
        /// Handle the disconnection of the player.
        /// </summary>
        /// <param name="clientId">Network object's id.</param>
        void OnPlayerQuit(ulong clientId)
        {
            Debug.Log("Removing player's objects, since it has leaved the game.");

            GetComponent<CharacterCreator>().DestroyPlayerObjects(clientId);
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > matchDurationInSeconds)
            {
                MatchFunctionality match = GetComponent<BaseGamemode>().GetCurrentMatch();
                match.End();

                Application.Quit();
            }
        }
    }
}
