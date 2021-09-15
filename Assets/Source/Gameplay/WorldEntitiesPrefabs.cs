using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class WorldEntitiesPrefabs : MonoBehaviour
    {
        [SerializeField, Tooltip("List containing prefabs which must have the component WorldEntity.")] 
        private List<GameObject> m_EntitiesPrefabs;

        private void OnEnable()
        {
            foreach (GameObject prefab in m_EntitiesPrefabs)
            {
                if (prefab.GetComponent<WorldEntity>() == null)
                {
                    throw new InvalidOperationException(
                        "WorldEntitiesPrefabs can only contain Prefabs with a WorldEntity component.");
                }
            }
        }

#nullable enable
        /// <summary>
        /// Looks for a prefab by its name.
        /// </summary>
        /// <param name="prefabName">Name of the prefab, which must be returned.</param>
        /// <returns>Prefab whose name is the one specified, null if there were no prefabs with that name.</returns>
        public GameObject? GetPrefabByName(string prefabName)
        {
            foreach (GameObject prefab in m_EntitiesPrefabs)
            {
                if (prefab.name.Equals(prefabName))
                {
                    return prefab;
                }
            }

            return null;
        }
#nullable disable
    }
}