using Balloondle.Client.UI.LoadingScene;
using Balloondle.Shared.Net.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Balloondle.Client
{
    /// <summary>
    /// Communicator with a Lobby server.
    /// 
    /// Provides a server based communication with Balloondle's storaging method, thus not
    /// providing direct access to the database to the clients.
    /// </summary>
    public class LobbyCommunicator : MonoBehaviour
    {
        /// <summary>
        /// Name of the GameObject containing the loading manager logic.
        /// </summary>
        private const string LOADING_MANAGER = "Loading Manager";

        /// <summary>
        /// Name of the GameObject containing the player preferences handler logic.
        /// </summary>
        private const string PLAYER_PREFERENCES_HANDLER = "Player Preferences Handler";

        /// <summary>
        /// Name of the GameObject containing the prerequirements retriever logic.
        /// </summary>
        private const string PREREQUIREMENTS_RETRIEVER = "Prerequirements Retriever";

        /// <summary>
        /// API request which creates an user containing the client's details.
        /// </summary>
        private const string SIGNUP_REQUEST = "/user/signup?";

        /// <summary>
        /// API request which initializes this client's instance.
        /// </summary>
        private const string LOGIN_REQUEST = "/user/login?";

        /// <summary>
        /// Base URL of the Rest API service.
        /// </summary>
        [SerializeField]
        private string basePage = "127.0.0.1:8000";

        /// <summary>
        /// User containing all the details passed by the lobby.
        /// </summary>
        private User userInstanceFromLobby;

        /// <summary>
        /// Check if authentication details are stored through the handler. If they are, proceed
        /// to try to login.
        /// </summary>
        async void Start()
        {
            GameObject playerPreferencesHandler = GameObject.Find(PLAYER_PREFERENCES_HANDLER);

            PlayerPreferencesHandler preferences = playerPreferencesHandler
                .GetComponent<PlayerPreferencesHandler>();

            if (preferences.HasRequiredAuthenticationDetails())
            {
                await Login();
            }
        }

        /// <summary>
        /// Register the name on the lobby.
        /// </summary>
        /// <param name="name">Name of the player.</param>
        async public void SignUp(string name)
        {
            string uri = $"{basePage}{SIGNUP_REQUEST}name={name}";
            Debug.Log($"URI: {uri}");
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Method = "POST";

            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

            HttpStatusCode statusCode = response.StatusCode;
            
            switch (statusCode)
            {
                case HttpStatusCode.Created:
                    HandleSignUpResponseStream(response.GetResponseStream());
                    break;
                case HttpStatusCode.BadRequest:
                    // TODO: Add retry possibility
                    Debug.Log("Invalid JSON data.");
                    break;
                default:
                    Debug.Log("Unexpected response.");
                    break;
            }
        }

        /// <summary>
        /// Obtains the data from the response's stream.
        /// </summary>
        /// <param name="responseStream">Stream of the response obtained from the lobby.</param>
        private void HandleSignUpResponseStream(Stream responseStream)
        {
            using StreamReader streamReader = new StreamReader(responseStream);
            using JsonTextReader responseReader = new JsonTextReader(streamReader);
            JsonSerializer serializer = new JsonSerializer();

            User user = serializer.Deserialize<User>(responseReader);

            PassAuthenticationDetailsToPlayerPreferencesHandler(user);
        }

        /// <summary>
        /// Call the player preferences handler to store the authentication details.
        /// </summary>
        /// <param name="user">User instance containing the authentication details.</param>
        private void PassAuthenticationDetailsToPlayerPreferencesHandler(User user)
        {
            GameObject playerPreferencesHandler = GameObject.Find(PLAYER_PREFERENCES_HANDLER);

            PlayerPreferencesHandler preferences = playerPreferencesHandler
                .GetComponent<PlayerPreferencesHandler>();

            Debug.Log($"Storing: {user.name}#{user.code}");
            preferences.StoreAuthenticationDetails(user.name, user.code);
        }

        /// <summary>
        /// Requests the initialization details.
        /// </summary>
        async public Task Login()
        {
            User user = GetAuthenticationDetailsFromPlayerPreferencesHandler();

            string uri = $"{basePage}{LOGIN_REQUEST}name={user.name}&code={user.code}";
            Debug.Log($"URI: {uri}");
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.Method = "GET";

            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

            HttpStatusCode statusCode = response.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.Accepted:
                    HandleLoginResponseStream(response.GetResponseStream());
                    break;
                case HttpStatusCode.BadRequest:
                    // TODO: Add retry possibility
                    Debug.Log("Incorrect authentication details.");
                    break;
                default:
                    Debug.Log("Unexpected response.");
                    break;
            }
        }

        /// <summary>
        /// Checks for the player preferences handler, and obtains the authentication details
        /// from it.
        /// </summary>
        /// <returns>User instance containing the authentication details.</returns>
        private User GetAuthenticationDetailsFromPlayerPreferencesHandler()
        {
            GameObject playerPreferencesHandler = GameObject.Find(PLAYER_PREFERENCES_HANDLER);

            PlayerPreferencesHandler preferences = playerPreferencesHandler
                .GetComponent<PlayerPreferencesHandler>();

            return preferences.GetUserFromAuthenticationDetails();
        }

        /// <summary>
        /// Retrieve the details obtained from the lobby through the response stream, proceeding to
        /// let the LoadingManager know that the prerequirements have been fulfilled.
        /// </summary>
        /// <param name="responseStream">Response stream provided by the request's reponse.</param>
        private void HandleLoginResponseStream(Stream responseStream)
        {
            using StreamReader streamReader = new StreamReader(responseStream);
            using JsonTextReader responseReader = new JsonTextReader(streamReader);
            JsonSerializer serializer = new JsonSerializer();

            userInstanceFromLobby = serializer.Deserialize<User>(responseReader);

            AskLoadingManagerToLoadLobbyScene();
        }

        /// <summary>
        /// Let the LoadingManager know the prerequirements have been fulfilled.
        /// </summary>
        private void AskLoadingManagerToLoadLobbyScene()
        {
            GameObject loadingManager = GameObject.Find(LOADING_MANAGER);
            LoadingManager manager = loadingManager.GetComponent<LoadingManager>();

            manager.OnPreparedToLoad();
        }

        /// <summary>
        /// Send the details retrieved from the lobby server, to the lobby scene.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the requiremenets retriever is
        /// missing.</exception>
        public void SendDetailsToLobbyScene()
        {
            GameObject prerequirementsRetriever = GameObject.Find(PREREQUIREMENTS_RETRIEVER);

            if (prerequirementsRetriever == null)
            {
                throw new InvalidOperationException($"Missing '{PREREQUIREMENTS_RETRIEVER}'");
            }

            PrerequirementsRetriever retriever = prerequirementsRetriever
                .GetComponent<PrerequirementsRetriever>();

            retriever.UserInstance = userInstanceFromLobby;
        }
    }
}