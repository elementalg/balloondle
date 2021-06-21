using UnityEngine;

namespace Balloondle.Server
{
    /// <summary>
    /// Loader of gamemodes, by creating a GameObject from their respective prefab.
    /// </summary>
    public class GamemodeLoader : MonoBehaviour
    {
        /// <summary>
        /// Prefab containing the test gamemode.
        /// </summary>
        [SerializeField]
        private GameObject freeForAllGamemodePrefab;

        /// <summary>
        /// Load a gamemode from its name.
        /// </summary>
        /// <param name="gamemode">Name of the gamemode to be loaded.</param>
        public void LoadGamemode(string gamemode)
        {
            if (gamemode.ToLower().Equals("dev"))
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