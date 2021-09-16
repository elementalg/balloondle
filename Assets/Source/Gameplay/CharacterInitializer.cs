using UnityEngine;

namespace Balloondle.Gameplay
{
    public class CharacterInitializer : MonoBehaviour
    {
        void Start()
        {
            WorldEntitySpawner worldEntitySpawner = GetComponent<WorldEntitySpawner>();
            WorldEntity balloon = worldEntitySpawner.Spawn("Balloon", Vector3.zero, Quaternion.identity);
            WorldEntity weapon = worldEntitySpawner.Spawn("GreenVenom",
                new Vector3(0, -2.95f, 0f), 
                Quaternion.Euler(0, 0, 180f));
            
            worldEntitySpawner.Attacher.Attach(balloon, new Vector3(0, -0.5f, 0f),
                weapon, new Vector3(0, -0.32f, 0f));
        }
    }
}
