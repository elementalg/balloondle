using System;
using Balloondle.UI.Controllers;
using UnityEngine;

namespace Balloondle.MiniGame
{
    public class MiniGameController : MonoBehaviour
    {
        [SerializeField, Tooltip("Prefab containing the joystick container.")]
        private GameObject m_JoystickContainerPrefab;

        [SerializeField, Tooltip("Prefab containing the HUD for the mini-game.")]
        private GameObject m_HUDPrefab;

        [SerializeField, Tooltip("Spawn boundaries for the barrels.")]
        private Rect m_BarrelSpawnBoundaries;
        
        private ScoreManager _scoreManager;
        private JoystickPointerListeningSurface _joystickContainer;
        private HUDController _hudController;
        private ActorsSpawner _actorsSpawner;
        
        public void StartGame()
        {
            _scoreManager = new ScoreManager();
            
            ShowHUD();
            StartActorsSpawner();           
            EnableJoystick();
        }

        private void EnableJoystick()
        {
            Canvas canvas = FindObjectOfType<Canvas>();

            if (canvas == null)
            {
                throw new InvalidOperationException("Canvas could not be found.");
            }
            
            _joystickContainer = Instantiate(m_JoystickContainerPrefab, canvas.transform)
                .GetComponent<JoystickPointerListeningSurface>();
        }

        private void ShowHUD()
        {
            Canvas canvas = FindObjectOfType<Canvas>();

            if (canvas == null)
            {
                throw new InvalidOperationException("Canvas could not be found.");
            }

            _hudController = Instantiate(m_HUDPrefab, canvas.transform).GetComponent<HUDController>();
        }

        private void StartActorsSpawner()
        {
            _actorsSpawner = gameObject.AddComponent<ActorsSpawner>();
            _actorsSpawner.Score = _scoreManager;
            _actorsSpawner.HUD = _hudController;
            _actorsSpawner.PointSpawnBoundaries = m_BarrelSpawnBoundaries;
        }
        
        public void EndGame()
        {
            Debug.Log("End.");
            
            _joystickContainer.DestroyJoystick();
            // Blur background.
            // Show retry button with obtained score, and also discord server button link.
        }
    }
}