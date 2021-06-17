using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Provides the base logic for the match.
    /// </summary>
    public class MatchFunctionality : MonoBehaviour
    {
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
    }
}
