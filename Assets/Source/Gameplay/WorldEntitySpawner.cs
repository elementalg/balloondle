using System;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class WorldEntitySpawner : MonoBehaviour
    {
        [SerializeField, Tooltip("WorldEntitiesPrefab containing the prefabs accessible by this spawner.")]
        private WorldEntitiesPrefabs m_WorldEntitiesPrefabs;
        
        [SerializeField, Tooltip("Whether or not world entity spawner supports attaching.")]
        private bool m_IsAttachingSupported;

        [SerializeField, Tooltip("Implementation which attaches entities.")]
        private WorldEntityAttacher m_Attacher;

        public bool IsAttachingSupported => m_IsAttachingSupported;

        public WorldEntityAttacher Attacher
        {
            get
            {
                if (!m_IsAttachingSupported)
                {
                    throw new NotSupportedException("Attaching is not supported.");
                }

                return m_Attacher;
            }
        }
        
        private void OnEnable()
        {
            if (m_WorldEntitiesPrefabs == null)
            {
                throw new InvalidOperationException("Spawner requires a non-null WorldEntitiesPrefabs.");
            }
            
            if (m_IsAttachingSupported)
            {
                if (m_Attacher == null)
                {
                    throw new InvalidOperationException(
                        "Must assign a WorldEntityAttacher in order to support attaching.");
                }
            }
        }

        /// <summary>
        /// Spawns the specified entity's prefab at the specified position with the specified rotation.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">if 'entityName' is null, empty or
        /// there is no prefab with that name.</exception>
        public GameObject Spawn(string entityName, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentException("Null or empty.", entityName);
            }
            
            GameObject prefab = m_WorldEntitiesPrefabs.GetPrefabByName(entityName) ??
                                 throw new ArgumentException($"No prefab with name '{entityName}'.", entityName);

            return Instantiate(prefab, position, rotation);
        }
    }
}
