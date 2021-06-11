using System.Net.Sockets;
using System.Text;
using UnityEngine;


namespace Balloondle
{
    public class Connector : MonoBehaviour
    {
        private TcpClient client;

        // Start is called before the first frame update
        void Start()
        {
            client = new TcpClient("127.0.0.1", 2202);

            if (client.Connected)
            {
                Debug.Log("Connected successfully.");
            }
        }

        // Update is called once per frame
        async void Update()
        {
            byte[] message = UTF8Encoding.UTF8.GetBytes("Hello from Client!");
            await client.GetStream().WriteAsync(message, 0, message.Length);

            byte[] receive = new byte[256];
            await client.GetStream().ReadAsync(receive, 0, 256);

            Debug.Log($"Received: {UTF8Encoding.UTF8.GetString(receive)}");
        }
    }
}
