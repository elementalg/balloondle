using UnityEngine;

namespace Balloondle.Gameplay.World
{
    public class PlayerConfusionInvoker : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Player>() != null)
            {
                other.gameObject.GetComponent<Player>().CooldownMovement();
            }
        }
    }
}