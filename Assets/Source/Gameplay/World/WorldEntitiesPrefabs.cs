using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Gameplay.World
{
    [CreateAssetMenu(fileName ="WorldEntitiesPrefabsData", menuName = "WorldEntities/Prefabs", order=1)]
    public class WorldEntitiesPrefabs : ScriptableObject
    {
        [SerializeField, Tooltip("List containing prefabs which must have the component WorldEntity.")] 
        public List<GameObject> m_EntitiesPrefabs;

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