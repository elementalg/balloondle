using Balloondle.Gameplay.World;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class CharacterInitializer : MonoBehaviour
    {
        private void Start()
        {
            WorldEntitySpawner worldEntitySpawner = GetComponent<WorldEntitySpawner>();
            WorldEntity balloon = worldEntitySpawner.Spawn("Balloon", new Vector3(45f, 1f), Quaternion.identity);
            WorldEntity weapon = worldEntitySpawner.Spawn("GreenVenom",
                new Vector3(45, -5.95f, 0f), 
                Quaternion.Euler(0, 0, 90f));
            WorldEntity weapon2 = worldEntitySpawner.Spawn("GreenVenom",
                new Vector3(50, -5.95f, 0f), 
                Quaternion.Euler(0, 0, 90f));

            Player player = balloon.GetComponent<Player>();
        }
    }
}
