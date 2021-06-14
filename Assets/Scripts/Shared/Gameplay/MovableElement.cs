using MLAPI;
using MLAPI.NetworkVariable;

namespace Balloondle.Shared
{
    public class MovableElement : NetworkBehaviour
    {
        private NetworkVariable<ulong> movableByClientId = new NetworkVariable<ulong>();

        public ulong MovableByClientId
        {
            get
            {
                return movableByClientId.Value;
            }
            set
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    movableByClientId.Value = value;
                }
            }
        }
    }
}
