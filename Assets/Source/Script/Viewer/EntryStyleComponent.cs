using Balloondle.Script.Core;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    [CreateAssetMenu(fileName = "EntryStyleComponent", menuName = "Script/Entry Style Component", order = 2)]
    public class EntryStyleComponent : ScriptableObject
    {
        public EntryType m_EntryType;
        public GameObject m_Prefab;

        public EntryStyleComponent()
        {
            m_EntryType = EntryType.Silence;
            m_Prefab = null;
        }
        
        public EntryStyleComponent(EntryType entryType, GameObject prefab)
        {
            m_EntryType = entryType;
            m_Prefab = prefab;
        }
    }
}