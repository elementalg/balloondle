using MLAPI;
using MLAPI.NetworkVariable;

namespace Balloondle.Shared
{
    /// <summary>
    /// Component used for easily identifying objects which are moved only from server side.
    /// </summary>
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
