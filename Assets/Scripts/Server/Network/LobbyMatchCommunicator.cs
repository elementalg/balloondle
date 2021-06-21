using System.Net;
using UnityEngine;

namespace Balloondle.Server
{
    public class LobbyMatchCommunicator : MonoBehaviour
    {
        private const string MATCH_STATE_READY = "/matchmaking/server_ready?";
        private const string MATCH_STATE_STOP = "/matchmaking/server_stop?";

        /// <summary>
        /// Base URL of the REST API service.
        /// </summary>
        private string basePage = "http://localhost:8000";

        private string map;
        private string gamemode;
        private long code;

        public void InitializeCommunicator(string map, string gamemode, long code, string lobbyBasePage)
        {
            this.map = map;
            this.gamemode = gamemode;
            this.code = code;
            basePage = lobbyBasePage;
        }

        async public void UpdateServerMatchStateToRunning()
        {
            string uri = $"{basePage}{MATCH_STATE_READY}map={map}&gamemode={gamemode}&code={code}";
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
                        Debug.Log("Updated state correctly.");
                        break;
                    case HttpStatusCode.BadRequest:
                        Debug.Log("Failed to update the state of the server.");
                        break;
                    default:
                        Debug.Log("Unknown response obtained.");
                        break;
                }
            } 
            catch (WebException ex)
            {
                Debug.Log($"Web error: {ex.Message}");
            }
        }

        async public void UpdateServerMatchStateToEnded()
        {
            string uri = $"{basePage}{MATCH_STATE_STOP}map={map}&gamemode={gamemode}&code={code}";
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
                        Debug.Log("Updated state correctly.");
                        break;
                    case HttpStatusCode.BadRequest:
                        Debug.Log("Failed to update the state of the server.");
                        break;
                    default:
                        Debug.Log("Unknown response obtained.");
                        break;
                }
            } 
            catch (WebException ex)
            {
                Debug.Log($"Web error: {ex.Message}");
            }
        }
    }
}