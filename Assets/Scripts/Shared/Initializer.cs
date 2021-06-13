using MLAPI;
using UnityEngine;

namespace Balloondle.Shared
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField]
        private GameObject serverLogicPrefab;

        [SerializeField]
        private GameObject clientLogicPrefab;

        [SerializeField]
        private bool isServer;

        // Start is called before the first frame update
        void Start()
        {
            if (isServer)
            {
                Instantiate(serverLogicPrefab);
            } 
            else
            {
                Instantiate(clientLogicPrefab);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
