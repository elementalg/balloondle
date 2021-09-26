using System;
using System.Collections.Generic;
using Balloondle.Gameplay.World;
using Balloondle.UI;
using Balloondle.UI.Controllers;
using UnityEngine;

namespace Balloondle.MiniGame
{
    public class MiniGameController : MonoBehaviour
    {
        [SerializeField]
        private Canvas m_SceneCanvas;
        
        [SerializeField, Tooltip("Prefab containing the joystick container.")]
        private GameObject m_JoystickContainerPrefab;

        [SerializeField, Tooltip("Prefab containing the HUD for the mini-game.")]
        private GameObject m_HUDPrefab;

        [SerializeField, Tooltip("Prefab containing the EventSystem.")]
        private GameObject m_EventSystemPrefab;
        
        [SerializeField, Tooltip("Prefab containing the GameOver UI.")]
        private GameObject m_GameOverPrefab;

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

        private int _currentObstacleStageIndex;
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
            _joystickContainer = Instantiate(m_JoystickContainerPrefab, m_SceneCanvas.transform)
                .GetComponent<JoystickPointerListeningSurface>();
        }

        private void ShowHUD()
        {
            _hudController = Instantiate(m_HUDPrefab, m_SceneCanvas.transform).GetComponent<HUDController>();
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
                if (m_ObstacleStages[_currentObstacleStageIndex + 1].m_ElapsedTimeRequired <= _elapsedTime)
                {
                    _currentObstacleStageIndex += 1;
                }
            }
            
            ObstacleStage stage = m_ObstacleStages[_currentObstacleStageIndex];
            
            if (_elapsedTimeAfterObstacleSpawn >= stage.m_SpawnEachSeconds)
            {
                for (int i = 0; i < stage.m_ObstaclesAmount; i++)
                {
                    _actorsSpawner.SpawnObstacle();
                }
                
                _elapsedTimeAfterObstacleSpawn = 0f;
            }
        }

        public void EndGame()
        {
            _joystickContainer.DestroyJoystick();
            Destroy(_hudController.gameObject);

            ShowGameOver();
        }

        private void ShowGameOver()
        {
            Instantiate(m_EventSystemPrefab);
            
            GameObject gameOver = Instantiate(m_GameOverPrefab, m_SceneCanvas.transform);
            GameOverSynchronizer synchronizer = gameOver.GetComponent<GameOverSynchronizer>();
            
            synchronizer.m_ScoreClamp.SetText(_scoreManager.CurrentScore.ToString("0."));

            if (!_scoreManager.IsHighScore())
            {
                synchronizer.m_HighScoreText.enabled = false;
            }
            
            _scoreManager.SaveScore();
        }
    }
}