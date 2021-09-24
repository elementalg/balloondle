using System;
using Balloondle.Gameplay.World;
using UnityEngine;

namespace Balloondle.MiniGame
{
    public class WorldLevelObstacle : MonoBehaviour
    {
        private bool _outOfBoundsOnce;

        public void HandleOutOfBounds()
        {
            if (GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("WorldEntity component is missing.");
            }
            
            WorldEntity entity = GetComponent<WorldEntity>();
            
            if (_outOfBoundsOnce)
            {
                entity.Damage(entity.Health);
                
                return;
            }

            _outOfBoundsOnce = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("WorldEntity component is missing.");
            }
            
            WorldEntity entity = GetComponent<WorldEntity>();
            entity.Damage(entity.Health);
        }
    }
}