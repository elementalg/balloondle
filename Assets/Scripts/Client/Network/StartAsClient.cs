using Balloondle.Shared.Game;
using MLAPI;
using MLAPI.Transports.UNET;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;

namespace Balloondle.Client.Network
{
    class StartAsClient : MonoBehaviour
    {
        void Start()
        {
            PlayerConnectionData playerConnectionData =
                new PlayerConnectionData("Simple0x47", "1100", "development");
            string jsonConnectionData = JsonConvert.SerializeObject(playerConnectionData);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding
                .UTF8
                .GetBytes(jsonConnectionData);

            NetworkManager.Singleton.StartClient();
        }

        void Update()
        {
            
        }
    }
}
