using UnityEngine;

namespace Balloondle.Client
{
    public class StatUIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject bulletObject;

        [SerializeField]
        private GameObject textObject;

        public GameObject Bullet { get => bulletObject; }
        public GameObject Text { get => textObject; }
    }
}
