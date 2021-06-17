using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Balloondle.Client
{
    /// <summary>
    /// Communicator with a Lobby server.
    /// 
    /// Provides a server based communication with Balloondle's storaging method, thus not
    /// providing direct access to the database to the clients.
    /// </summary>
    public class LobbyCommunicatorFunctionality : MonoBehaviour
    {
        /// <summary>
        /// API request which initializes this client's instance.
        /// </summary>
        private const string LOGIN_REQUEST = "/user/login?";

        /// <summary>
        /// API request which creates an user containing the client's details.
        /// </summary>
        private const string SIGNUP_REQUEST = "/user/signup?";

        /// <summary>
        /// Base URL of the Rest API service.
        /// </summary>
        [SerializeField]
        private string basePage = "127.0.0.1:8000";

        /// <summary>
        /// Register the name on the lobby.
        /// </summary>
        /// <param name="name">Name of the player.</param>
        public void SignUp(string name)
        {
            // Send the request on a coroutine in order to avoid
            // freezing the main thread.
            string uri = $"{basePage}{SIGNUP_REQUEST}name={name}";
            Debug.Log($"URI: {uri}");
            StartCoroutine(SignUpRequest(uri));
        }

        /// <summary>
        /// Proceed to send the request to the lobby.
        /// </summary>
        /// <param name="uri">Final REST API request url.</param>
        /// <returns></returns>
        IEnumerator SignUpRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.Log($"Error: {webRequest.result}");
                        Debug.Log($"Error message: {webRequest.error}");
                        throw new InvalidOperationException("Could not retrieve data from the Lobby.");
                    case UnityWebRequest.Result.Success:
                        Debug.Log($"Success: {webRequest.downloadHandler.text}");
                        string userJson = webRequest.downloadHandler.text;

                        Debug.Log($"UserJson: {userJson}");
                        //JObject user = JObject.Parse(userJson);
                        //string code = (string)user["code"];

                        //PlayerPrefs.SetString("code", code);
                        break;
                    case UnityWebRequest.Result.InProgress:
                        Debug.Log($"InProgress");
                        break;
                    default:
                        Debug.Log($"Undetected result.");
                        break;
                }
            }
        }

        /// <summary>
        /// Requests the initialization details.
        /// </summary>
        public void Startup()
        {
            string name = PlayerPrefs.GetString("name");
            string code = PlayerPrefs.GetString("code");

            StartCoroutine(StartupRequest($"{basePage}{LOGIN_REQUEST}name={name}?code={code}"));
        }

        /// <summary>
        /// Proceed to send the request to the lobby.
        /// </summary>
        /// <param name="uri">Final REST API request url.</param>
        /// <returns></returns>
        IEnumerator StartupRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.Log($"Error: {webRequest.result}");
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log($"Success: {webRequest.downloadHandler.text}");
                        break;
                    case UnityWebRequest.Result.InProgress:
                        Debug.Log($"InProgress");
                        break;
                    default:
                        Debug.Log($"Undetected result.");
                        break;
                }
            }
        }
    }
}