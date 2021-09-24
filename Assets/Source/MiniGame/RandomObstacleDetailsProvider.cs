using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Balloondle.MiniGame
{
    /// <summary>
    /// Generates randomly details for the obstacles.
    /// </summary>
    public class RandomObstacleDetailsProvider
    {
        private List<Rect> _spawningRects;
        private readonly float _minForce;
        private readonly float _maxForce;

        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 Force { get; private set; }

        public RandomObstacleDetailsProvider(List<Rect> spawningRects, float minForce, float maxForce)
        {
            _spawningRects = spawningRects;
            _minForce = minForce;
            _maxForce = maxForce;
        }

        public void Generate()
        {
            // RandomLocation: 0 - left, 1 - top, 2 - right, 3 - bottom.
            int randomLocation = Random.Range(0, 4);
            Direction = GetDirectionForLocation(randomLocation);

            Rect spawningRect = _spawningRects[randomLocation];
            Position = new Vector3(Random.Range(spawningRect.xMin, spawningRect.xMax),
                Random.Range(spawningRect.yMin, spawningRect.yMax));

            Rotation = GetRotationForLocation(randomLocation);

            Force = Direction * Random.Range(_minForce, _maxForce);
        }

        private Vector2 GetDirectionForLocation(int randomLocation)
        {
            Vector2 direction = new Vector2();
            float delta = Random.Range(-0.05f, 0.05f);
            
            switch (randomLocation)
            {
                case 0:
                    direction.Set(1f, delta);
                    break;
                case 1:
                    direction.Set(delta, -1f);
                    break;
                case 2:
                    direction.Set(-1f, delta);
                    break;
                case 3:
                    direction.Set(delta, 1f);
                    break;
                default:
                    direction.Set(1f, delta);
                    break;
            }
            
            direction.Normalize();

            return direction;
        }

        private Quaternion GetRotationForLocation(int randomLocation)
        {
            Quaternion rotation;
            
            switch (randomLocation)
            {
                case 0:
                    rotation = Quaternion.identity;
                    break;
                case 1:
                    rotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                case 2:
                    rotation = Quaternion.Euler(0f, 0f, -180f);
                    break;
                case 3:
                    rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                default:
                    rotation = Quaternion.identity;
                    break;
            }

            return rotation;
        }
    }
}