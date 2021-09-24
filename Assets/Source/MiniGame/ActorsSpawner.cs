using System;
using Balloondle.Gameplay;
using Balloondle.Gameplay.World;
using Balloondle.Script;
using UnityEngine;

namespace Balloondle.MiniGame
{
    public class ActorsSpawner : MonoBehaviour
    {
        private const string PlayerEntityName = "Balloon";
        private const string PlayerWeaponEntityName = "GreenVenom";
        private const string PointEntityName = "Barrel";
        
        private WorldEntitySpawner _spawner;

        public ScoreManager Score { get; set; }
        public HUDController HUD { get; set; }
        
        private void Start()
        {
            if (FindObjectOfType<WorldEntitySpawner>() == null)
            {
                throw new InvalidOperationException("Missing WorldEntitySpawner from scene.");
            }
            
            _spawner = FindObjectOfType<WorldEntitySpawner>();
            
            SpawnPlayer();
            SpawnWeapon();
            SpawnPointEntity();
        }

        public void SpawnPlayer()
        {
            WorldEntity player = _spawner.Spawn(PlayerEntityName, new Vector3(8.47f, 1.79f, 0f), Quaternion.identity);
            player.m_Indestructible = false;
            player.OnPreDestroy += damage =>
            {
                Debug.Log("Player - OnPreDestroy");
                FindObjectOfType<ScriptDirector>().TryTriggerExpireEvent("player-destroyed");

                Player playerLogic = player.GetComponent<Player>();

                if (playerLogic.HasWeapon())
                {
                    playerLogic.DropWeapon();
                }
            };
            
            player.OnDamagePreReceived += damage =>
            {
                Debug.Log($"Player - OnDamagePreReceived {player.Health} after {player.Health - damage}");
                HUD.UpdateHealth(player.MaxHealth, Mathf.Max(0f, player.Health - damage));
            };
        }

        public void SpawnWeapon()
        {
            WorldEntity weapon = _spawner.Spawn(PlayerWeaponEntityName, new Vector3(10.47f, 1.79f, 0f),
                Quaternion.identity);
        }
        
        public void SpawnPointEntity()
        {
            WorldEntity pointEntity = _spawner.Spawn(PointEntityName, new Vector3(3.8f, -15f, 0f),
                Quaternion.identity);

            pointEntity.OnPreDestroy += damage =>
            {
                Score.Score();
                HUD.UpdateScore(Score.CurrentScore);
                
                SpawnPointEntity();
            };
        }
    }
}