using Balloondle.Script.Core;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    [CreateAssetMenu(fileName = "EntryStyleComponent", menuName = "Script/Entry Style Component", order = 2)]
    public class EntryStyleComponent : ScriptableObject
    {
        public Type m_EntryType;
        public GameObject m_Prefab;

        public EntryStyleComponent()
        {
            m_EntryType = Type.Silence;
            m_Prefab = null;
        }
        
        public EntryStyleComponent(Type entryType, GameObject prefab)
        {
            m_EntryType = entryType;
            m_Prefab = prefab;
        }
    }
}