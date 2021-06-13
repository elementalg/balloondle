using UnityEngine;

namespace Balloondle.Client
{
    public class PlayerControllerFunctionality : MonoBehaviour
    {
        public GameObject Player { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Player != null)
            {
                Vector3 position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10f);

                GetComponent<Camera>().transform.SetPositionAndRotation(position, Quaternion.identity);
            }
        }
    }
}
