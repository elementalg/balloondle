using Balloondle.Shared.Network.Game;
using MLAPI;
using Newtonsoft.Json;
using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Provides the base logic for the match.
    /// </summary>
    public class MatchFunctionality : MonoBehaviour
    {
        /// <summary>
        /// Tag used for identifying the instance of the lobby match communicator.
        /// </summary>
        private const string LOBBY_MATCH_COMMUNICATOR_TAG = "LobbyMatchCommunicator";

        /// <summary>
        /// Prefab containing the MapLoader and GamemodeLoader.
        /// </summary>
        [SerializeField]
        private GameObject loaderPrefab;

        public enum MatchState
        {
            PAUSE,
            RUNNING,
            ENDED,
        }

        /// <summary>
        /// Name of the current map.
        /// </summary>
        public string Map { get; set; }
        /// <summary>
        /// Name of the current gamemode.
        /// </summary>
        public string Gamemode { get; set; }
        /// <summary>
        /// Amount of maximum allowed players.
        /// </summary>
        public uint MaxPlayers { get; set; }

        public MatchState State { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            State = MatchState.PAUSE;

            GameObject loader = GameObject.Instantiate(loaderPrefab);
            loader.GetComponent<MapLoader>().LoadMap(Map);
            loader.GetComponent<GamemodeLoader>().LoadGamemode(Gamemode);
        }

        public void End()
        {
            GameObject gamemodeObject = GameObject.FindGameObjectWithTag("Gamemode");
            StatsFollower statsFollower = gamemodeObject.GetComponent<StatsFollower>();
            statsFollower.CalculatePlayersLeaderboardPosition();
            string serializedStats = JsonConvert.SerializeObject(statsFollower.PlayerStats);

            GameObject synchronizer = GameObject.Find("Messenger");
            synchronizer.GetComponent<NetworkRpcMessages>().OnMatchEndsClientRpc(serializedStats);

            GameObject lobbyMatchCommunicator = GameObject.FindGameObjectWithTag(LOBBY_MATCH_COMMUNICATOR_TAG);
            LobbyMatchCommunicator communicator = lobbyMatchCommunicator.GetComponent<LobbyMatchCommunicator>();

            communicator.UpdateServerMatchStateToEnded();
        }
    }
}
