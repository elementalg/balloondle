﻿using System;
using Balloondle.Gameplay.World;
using Balloondle.Script;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Balloondle.MiniGame
{
    public class ActorsSpawner
    {
        private const string PlayerEntityName = "Balloon";
        private const string PlayerWeaponEntityName = "GreenVenom";
        private const string PointEntityName = "Barrel";
        private const string ObstacleEntityName = "Arrow";
        private const float MinimumDistanceBetweenPlayerAndPoint = 2f;
        
        private WorldEntitySpawner _spawner;
        
        private ScoreManager _score;
        private HUDController _hud;
        private Rect _pointSpawnBoundaries;
        private RandomObstacleDetailsProvider _randomObstacleDetailsProvider;

        private WorldEntity _currentPlayer;

        public ActorsSpawner(WorldEntitySpawner spawner, ScoreManager score, HUDController hud, 
            Rect pointSpawnBoundaries, RandomObstacleDetailsProvider detailsProvider)
        {
            _spawner = spawner;
            _score = score;
            _hud = hud;
            _pointSpawnBoundaries = pointSpawnBoundaries;
            _randomObstacleDetailsProvider = detailsProvider;
        }
        
        public void Start()
        {
            SpawnPlayer();
            SpawnWeapon();
            SpawnPointEntity();
        }

        private void SpawnPlayer()
        {
            WorldEntity player = _spawner.Spawn(PlayerEntityName, new Vector3(8.47f, 1.79f, 0f), Quaternion.identity);
            player.m_Indestructible = false;
            player.OnPreDestroy += damage =>
            {
                GameObject.FindObjectOfType<ScriptDirector>().TryTriggerExpireEvent("player-destroyed");

                Player playerLogic = player.GetComponent<Player>();

                if (playerLogic.HasWeapon())
                {
                    playerLogic.DropWeapon();
                }
            };
            
            player.OnDamagePreReceived += damage =>
            {
                _hud.UpdateHealth(player.MaxHealth, Mathf.Max(0f, player.Health - damage));
            };

            _currentPlayer = player;
        }

        private void SpawnWeapon()
        {
            WorldEntity weapon = _spawner.Spawn(PlayerWeaponEntityName, new Vector3(10.47f, 1.79f, 0f),
                Quaternion.identity);
        }
        
        private void SpawnPointEntity()
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(_pointSpawnBoundaries.xMin, _pointSpawnBoundaries.xMax),
                Random.Range(_pointSpawnBoundaries.yMin, _pointSpawnBoundaries.yMax));

            if (Vector2.Distance(_currentPlayer.transform.position, randomPosition) <
                MinimumDistanceBetweenPlayerAndPoint)
            {
                SpawnPointEntity();
                return;
            }
            
            Quaternion randomAngle = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            
            WorldEntity pointEntity = _spawner.Spawn(PointEntityName, randomPosition, randomAngle);

            pointEntity.OnPreDestroy += damage =>
            {
                _score.Score();
                _hud.UpdateScore(_score.CurrentScore);
                
                SpawnPointEntity();
            };
        }

        public void SpawnObstacle()
        {
            _randomObstacleDetailsProvider.Generate();

            WorldEntity obstacleEntity = _spawner.Spawn(ObstacleEntityName, 
                _randomObstacleDetailsProvider.Position, _randomObstacleDetailsProvider.Rotation);

            if (obstacleEntity.GetComponent<Rigidbody2D>() == null)
            {
                throw new InvalidOperationException("Obstacle entity is missing a Rigidbody2D component.");
            }
            
            Rigidbody2D body = obstacleEntity.GetComponent<Rigidbody2D>();
            body.AddForce(_randomObstacleDetailsProvider.Force, ForceMode2D.Impulse);
        }
    }
}