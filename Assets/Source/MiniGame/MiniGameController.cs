using System;
using Balloondle.Gameplay.World;
using Balloondle.Script;
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
        
        private ScoreManager _scoreManager;
        private JoystickPointerListeningSurface _joystickContainer;
        private HUDController _hudController;
        
        public void StartGame()
        {
            _scoreManager = new ScoreManager();
            
            ShowHUD();
            SpawnPlayer();
            EnableJoystick();
        }

        private void SpawnPlayer()
        {
            WorldEntitySpawner spawner = FindObjectOfType<WorldEntitySpawner>();

            WorldEntity player = spawner.Spawn("Balloon", new Vector3(8.47f, 1.79f, 0f), Quaternion.identity);
            player.m_Indestructible = false;
            player.OnPreDestroy += damage =>
            {
                FindObjectOfType<ScriptDirector>().TryTriggerExpireEvent("player-destroyed");
            };

            player.OnDamagePreReceived += damage =>
            {
                _hudController.UpdateHealth(player.MaxHealth, Mathf.Min(0f, player.Health - damage));
            };
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
            GameObject hud = Instantiate(m_HUDPrefab);

            if (hud.GetComponent<HUDController>() == null)
            {
                throw new InvalidOperationException("Missing HUDController from HUD prefab.");
            }
            
            _hudController = hud.GetComponent<HUDController>();
        }

        public void EndGame()
        {
            
        }
    }
}