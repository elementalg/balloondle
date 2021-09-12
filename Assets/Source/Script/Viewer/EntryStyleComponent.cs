using Balloondle.Script.Core;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    public class EntryStyleComponent : MonoBehaviour
    {
        public string m_Name;
        public EntryType m_EntryType;
        public GameObject m_Prefab;

        public EntryStyleComponent()
        {
            m_Name = "";
            m_EntryType = EntryType.Silence;
            m_Prefab = null;
        }
        
        public EntryStyleComponent(string name, EntryType entryType, GameObject prefab)
        {
            m_Name = name;
            m_EntryType = entryType;
            m_Prefab = prefab;
        }
    }
}