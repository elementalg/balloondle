using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField, Tooltip("Prefab of the character desired to be spawned.")] 
        private GameObject m_CharacterPrefab;

        public GameObject Spawn(Vector3 position, Quaternion angle)
        {
            return Instantiate(m_CharacterPrefab, position, angle);
        }
    }
}
