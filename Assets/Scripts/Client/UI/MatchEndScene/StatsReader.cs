using Balloondle.Shared.Net.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.Client
{
    public class StatsReader : MonoBehaviour
    {
        private const string PLAYER_MATCH_STATS_PREF = "player_match_stats";

        [SerializeField]
        private GameObject parentCanvas;

        [SerializeField]
        private GameObject statPrefab;

        void Start()
        {
            string playerStats = PlayerPrefs.GetString(PLAYER_MATCH_STATS_PREF);
            PlayerMatchStats stats = JsonConvert.DeserializeObject<PlayerMatchStats>(playerStats);

            CreateUIStateDamage(stats.damage);
            CreateUIStatePosition(stats.position);

            PlayerPrefs.DeleteKey(PLAYER_MATCH_STATS_PREF);
        }

        private void CreateUIStateDamage(float damage)
        {
            GameObject uiState = GameObject.Instantiate(statPrefab, parentCanvas.transform);
            uiState.transform.localPosition = new Vector3(0f, 0f, 0f);

            Text text = uiState.transform.Find("Text").GetComponent<Text>();
            text.text = $"Damage done: {damage}";
        }

        private void CreateUIStatePosition(uint position)
        {
            GameObject uiState = GameObject.Instantiate(statPrefab, parentCanvas.transform);
            uiState.transform.localPosition = new Vector3(0f, -150f, 0f);

            Text text = uiState.transform.Find("Text").GetComponent<Text>();

            string leaderText = string.Format("Leaderboard position: {0}", position + 1);
            text.text = leaderText;
        }
    }
}