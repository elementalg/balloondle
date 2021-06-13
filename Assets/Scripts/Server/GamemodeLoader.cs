using UnityEngine;

namespace Balloondle.Server
{
    public class GamemodeLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject freeForAllGamemodePrefab;

        public void LoadGamemode(string gamemode)
        {
            if (gamemode.ToLower().Equals("freeforall"))
            {
                GameObject.Instantiate(freeForAllGamemodePrefab);
            }
            else
            {
                throw new System.ArgumentException("Invalid gamemode name.");
            }
        }
    }
}
