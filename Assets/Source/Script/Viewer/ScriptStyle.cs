using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    [CreateAssetMenu(fileName = "ScriptStyle", menuName = "Script/Style", order = 1)]
    public class ScriptStyle : ScriptableObject
    {
        public string m_StyleName;
        public List<EntryStyleComponent> m_Components;

        private void Start()
        {
            if (m_Components == null)
            {
                m_Components = new List<EntryStyleComponent>();
            }
        }
    }
}