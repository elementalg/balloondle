using System;
using UnityEngine;

namespace Balloondle.Gameplay.World
{
    public class WorldEntitySpawner : MonoBehaviour
    {
        [SerializeField, Tooltip("WorldEntitiesPrefab containing the prefabs accessible by this spawner.")]
        private WorldEntitiesPrefabs m_WorldEntitiesPrefabs;
        
        [SerializeField, Tooltip("Whether or not world entity spawner supports attaching.")]
        private bool m_IsAttachingSupported;

        /// <summary>
        /// Assigned statically to <see cref="WorldEntity"/>.
        /// </summary>
        [SerializeField, Tooltip("Implementation which attaches entities.")]
        private WorldEntityAttacher m_Attacher;
        
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

                WorldEntity.Attacher = m_Attacher;
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
        public WorldEntity Spawn(string entityName, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentException("Null or empty.", entityName);
            }
            
            GameObject prefab = m_WorldEntitiesPrefabs.GetPrefabByName(entityName) ??
                                 throw new ArgumentException(
                                     $"No prefab with name '{entityName}'.", nameof(entityName));

            if (prefab.GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("Prefab does not contain a WorldEntity component.");
            }

            WorldEntity spawnedEntity = Instantiate(prefab, position, rotation).GetComponent<WorldEntity>();

            return spawnedEntity;
        }
    }
}
