using Balloondle.Script.Core;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    [CreateAssetMenu(fileName = "EntryStyleComponent", menuName = "Script/Entry Style Component", order = 2)]
    public class StyleComponent : ScriptableObject
    {
        public Type m_EntryType;
        public GameObject m_Prefab;

        public StyleComponent()
        {
            m_EntryType = Type.Silence;
            m_Prefab = null;
        }
        
        public StyleComponent(Type entryType, GameObject prefab)
        {
            m_EntryType = entryType;
            m_Prefab = prefab;
        }
    }
}