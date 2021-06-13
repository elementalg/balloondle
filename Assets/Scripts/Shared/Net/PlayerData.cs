using MLAPI;
using MLAPI.NetworkVariable;

namespace Balloondle.Shared
{
    public class PlayerData : NetworkBehaviour
    {
        public NetworkVariable<string> playerName;
        public NetworkVariable<ulong> clientId;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
