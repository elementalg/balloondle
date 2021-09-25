using System;
using System.Collections.Generic;
using Balloondle.Gameplay.World;
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

        [SerializeField, Tooltip("Obstacle spawn zones.")]
        private List<Rect> m_ObstacleSpawnZones;

        [SerializeField, Tooltip("Minimum (x) and maximum (y) force which can be applied to the obstacles.")]
        private Vector2 m_MinAndMaxForceForObstacle;

        [SerializeField] 
        private List<ObstacleStage> m_ObstacleStages;

        private ScoreManager _scoreManager;
        private JoystickPointerListeningSurface _joystickContainer;
        private HUDController _hudController;
        private ActorsSpawner _actorsSpawner;
        
        private float _elapsedTime;

        private int _currentObstacleStageIndex = 0;
        private float _elapsedTimeAfterObstacleSpawn;

        private void Start()
        {
            if (m_ObstacleStages.Count == 0)
            {
                throw new InvalidOperationException("ObstaclesStages cannot be empty.");
            }
        }

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
            if (FindObjectOfType<WorldEntitySpawner>() == null)
            {
                throw new InvalidOperationException("Missing WorldEntitySpawner component.");
            }
            
            WorldEntitySpawner worldEntitySpawner = FindObjectOfType<WorldEntitySpawner>();
            RandomObstacleDetailsProvider detailsProvider = new RandomObstacleDetailsProvider(m_ObstacleSpawnZones,
                m_MinAndMaxForceForObstacle.x, m_MinAndMaxForceForObstacle.y);

            _actorsSpawner = new ActorsSpawner(worldEntitySpawner, _scoreManager, _hudController,
                m_BarrelSpawnBoundaries, detailsProvider);
            _actorsSpawner.Start();
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            _elapsedTimeAfterObstacleSpawn += Time.deltaTime;

            if (m_ObstacleStages.Count > _currentObstacleStageIndex + 1)
            {
                if (m_ObstacleStages[_currentObstacleStageIndex + 1].ElapsedTimeRequired <= _elapsedTime)
                {
                    _currentObstacleStageIndex += 1;
                }
            }
            
            ObstacleStage stage = m_ObstacleStages[_currentObstacleStageIndex];
            
            if (_elapsedTimeAfterObstacleSpawn >= stage.SpawnEachSeconds)
            {
                for (int i = 0; i < stage.ObstaclesAmount; i++)
                {
                    _actorsSpawner.SpawnObstacle();
                }
                
                _elapsedTimeAfterObstacleSpawn = 0f;
            }
        }

        public void EndGame()
        {
            Debug.Log("End");
            _joystickContainer.DestroyJoystick();
            // Blur background.
            // Show retry button with obtained score, and also discord server button link.
        }
    }
}