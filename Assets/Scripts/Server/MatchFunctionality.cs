using UnityEngine;

namespace Balloondle.Server
{
    public class MatchFunctionality : MonoBehaviour
    {
        [SerializeField]
        private GameObject loaderPrefab;

        public enum MatchState
        {
            PAUSE,
            RUNNING,
            ENDED,
        }

        public string Map { get; set; }
        public string Gamemode { get; set; }
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

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
