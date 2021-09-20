using Balloondle.Gameplay.World;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class CharacterInitializer : MonoBehaviour
    {
        private void Start()
        {
            WorldEntitySpawner worldEntitySpawner = GetComponent<WorldEntitySpawner>();
            WorldEntity balloon = worldEntitySpawner.Spawn("Balloon", Vector3.zero, Quaternion.identity);
            WorldEntity weapon = worldEntitySpawner.Spawn("GreenVenom",
                new Vector3(0, -2.95f, 0f), 
                Quaternion.Euler(0, 0, 180f));

            Player player = balloon.GetComponent<Player>();
        }
    }
}
