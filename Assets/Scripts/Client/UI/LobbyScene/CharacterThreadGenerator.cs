using Balloondle.Client.UI.LobbyScene;
using UnityEngine;

namespace Balloondle.Client
{
    public class CharacterThreadGenerator : MonoBehaviour
    {
        /// <summary>
        /// GameObject which contains the balloon's sprite and rigid body.
        /// </summary>
        [SerializeField]
        private GameObject balloon;

        /// <summary>
        /// GameObject which contains the weapon's sprite and rigid body.
        /// </summary>
        [SerializeField]
        private GameObject weapon;

        /// <summary>
        /// Point of the balloon where to tie the thread to.
        /// </summary>
        [SerializeField]
        private Vector2 balloonAnchorPoint;

        /// <summary>
        /// Point of the weapon where to tie the thread to.
        /// </summary>
        [SerializeField]
        private Vector2 weaponAnchorPoint;

        /// <summary>
        /// Prefab used for generating thread cells.
        /// </summary>
        [SerializeField]
        private GameObject threadCellPrefab;

        /// <summary>
        /// Size of a single thread cell.
        /// </summary>
        [SerializeField]
        private float threadCellSize;

        private void Start()
        {
            ThreadCellsGenerator cellsGenerator = new ThreadCellsGenerator(balloon, balloonAnchorPoint, 
                weapon, weaponAnchorPoint, threadCellPrefab, threadCellSize);

            cellsGenerator.GenerateThreadCells();
        }
    }
}
