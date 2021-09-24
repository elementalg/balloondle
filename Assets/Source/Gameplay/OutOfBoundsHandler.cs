using Balloondle.Gameplay.World;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class OutOfBoundsHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Move player to spawn.
            if (other.gameObject.GetComponent<Player>() != null)
            {
                other.gameObject.transform.SetPositionAndRotation(new Vector3(8.47f, 1.79f, 0f), Quaternion.identity);
                return;
            }

            // Move weapon to spawn.
            if (other.gameObject.GetComponent<Weapon>() != null)
            {
                other.gameObject.transform.SetPositionAndRotation(new Vector3(10.47f, 1.79f, 0f), Quaternion.identity);
                return;
            }

            if (other.gameObject.GetComponent<WorldEntity>() != null)
            {
                WorldEntity entity = other.gameObject.GetComponent<WorldEntity>();
                entity.Damage(entity.Health);
                return;
            }
        }
    }
}