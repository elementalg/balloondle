using Balloondle.Client.UI.LoadingScene;
using Balloondle.Shared;
using Balloondle.Shared.Net.Models;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Balloondle.Client
{
    public class LobbyMatchmaking : MonoBehaviour
    {
        private const string SEARCH_REQUEST = "/matchmaking/search?";
        private const string SEARCH_ALIVE_PING = "/matchmaking/search_alive?";
        private const string STOP_SEARCH_REQUEST = "/matchmaking/stop_search?";

        /// <summary>
        /// Name of the GameObject containing the prerequirements retriever logic.
        /// </summary>
        private const string PREREQUIREMENTS_RETRIEVER = "Prerequirements Retriever";

        private const string DEVELOPMENT_MAP = "dev";
        private const string DEVELOPMENT_GAMEMODE = "dev";

        /// <summary>
        /// Base URL of the REST API service.
        /// </summary>
        [SerializeField]
        private string basePage = "http://localhost:8000";

        [SerializeField]
        private float alivePingSeconds = 5f;

        private float alivePingElapsed = 0f;
        private bool alivePing = false;

        private User user;

        async public void StartSearchingMatchAsync()
        {
            GetComponent<MatchSearchUpdater>().ShowMatchSearchStatus();

            GameObject prerequirementsRetriever = GameObject.Find(PREREQUIREMENTS_RETRIEVER);
            PrerequirementsRetriever retriever = prerequirementsRetriever
                .GetComponent<PrerequirementsRetriever>();

            user = retriever.UserInstance;

            if (user == null)
            {
                user = new User();
                user.name = PlayerPrefs.GetString(PlayerPreferencesHandler.NAME_KEY);
                user.code = uint.Parse(PlayerPrefs.GetString(PlayerPreferencesHandler.CODE_KEY));
            }

            string uri = $"{basePage}{SEARCH_REQUEST}name={user.name}&code={user.code}" +
                $"&map={DEVELOPMENT_MAP}&gamemode={DEVELOPMENT_GAMEMODE}";
            Debug.Log(uri);
            
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Method = "POST";

            try
            {
                using HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

                HttpStatusCode statusCode = response.StatusCode;

                switch (statusCode)
                {
                    case HttpStatusCode.Created:
                        Debug.Log("Started searching...");

                        // Start sending search alive pings, in order to get the match details in case
                        // there's a match available.
                        alivePing = true;
                        alivePingElapsed = 0f;
                        break;
                    case HttpStatusCode.BadRequest:
                        Debug.Log("Failed to start searching...");
                        break;
                    default:
                        Debug.Log("Server error.");
                        break;
                }
            } 
            catch (WebException ex)
            {
                Debug.Log($"Web error: {ex.Message}");
            }
        }

        async void Update()
        {
            if (alivePing)
            {
                if (alivePingElapsed > alivePingSeconds)
                {
                    alivePingElapsed = 0f;
                    await SearchAlivePingAsync();
                    alivePingElapsed = 0f;
                } 
                else
                {
                    alivePingElapsed += Time.deltaTime;
                }
            }    
        }

        async Task SearchAlivePingAsync()
        {
            string uri = $"{basePage}{SEARCH_ALIVE_PING}name={user.name}&code={user.code}";
            Debug.Log(uri);

            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Method = "POST";

            try
            {
                using HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

                HttpStatusCode statusCode = response.StatusCode;

                switch (statusCode)
                {
                    case HttpStatusCode.Accepted:
                        GetComponent<MatchSearchUpdater>().HideAll();
                        HandleSearchAliveResponseStream(response.GetResponseStream());

                        alivePing = false;
                        break;
                    case HttpStatusCode.OK:
                        Debug.Log("Sent correctly alive ping. No match found...");
                        break;
                    case HttpStatusCode.BadRequest:
                        Debug.Log("Error, incorrect details.");
                        break;
                    default:
                        Debug.Log("Server error.");
                        break;
                }
            } 
            catch (WebException webException)
            {
                Debug.Log($"Web error: {webException.Message}");
            }

            
        }

        private void HandleSearchAliveResponseStream(Stream responseStream)
        {
            using StreamReader streamReader = new StreamReader(responseStream);
            using JsonTextReader responseReader = new JsonTextReader(streamReader);
            JsonSerializer serializer = new JsonSerializer();

            MatchServerDetails details = serializer.Deserialize<MatchServerDetails>(responseReader);
            Debug.Log($"Match details: {details.server_ip} : {details.server_port}");

            PlayerPrefs.SetString("ip", details.server_ip);
            PlayerPrefs.SetInt("port", int.Parse(details.server_port));
            PlayerPrefs.Save();

            SceneManager.LoadScene(Scenes.GameScene.ToString(), LoadSceneMode.Single);
        }

        async public void StopSearchingMatchAsync()
        {
            GetComponent<MatchSearchUpdater>().HideMatchSearchStatus();

            if (user == null)
            {
                Debug.Log("Tried to stop searching match, without starting the search first.");
                return;
            }

            alivePing = false;
            string uri = $"{basePage}{STOP_SEARCH_REQUEST}name={user.name}&code={user.code}";
            Debug.Log(uri);

            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
                request.Method = "POST";

                using HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                Debug.Log("Stop response.");
                HttpStatusCode statusCode = response.StatusCode;

                switch (statusCode)
                {
                    case HttpStatusCode.Accepted:
                        Debug.Log("Stopped searching.");
                        break;
                    case HttpStatusCode.BadRequest:
                        Debug.Log("Failed to stop searching...");
                        break;
                    default:
                        Debug.Log("Server error.");
                        break;
                }
            } 
            catch (WebException ex)
            {
                Debug.Log($"Web error: {ex.Message}");
            }

            Debug.Log("Stopped searching.");

        }

        // TODO Test the matchmaking
    }
}