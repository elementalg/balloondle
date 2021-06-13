using UnityEngine;

namespace Balloondle.Server
{
    public class FreeForAllGamemode : MonoBehaviour
    {
        [SerializeField]
        private int minimumRequiredPlayers = 1;

        private int playersCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<BaseGamemode>().OnPlayerJoin += OnPlayerJoin;
            GetComponent<BaseGamemode>().OnPlayerQuit += OnPlayerQuit;
        }

        void OnPlayerJoin(ulong clientId)
        {
            Debug.Log("OnPlayerJoin");

            ++playersCount;

            MatchFunctionality match = GetComponent<BaseGamemode>().GetCurrentMatch();

            Debug.Log($"MatchState: {match.State}");

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

        void OnPlayerQuit(ulong clientId)
        {
            Debug.Log("OnPlayerQuit");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
