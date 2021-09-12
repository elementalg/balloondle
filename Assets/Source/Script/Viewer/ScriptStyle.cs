using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    public class ScriptStyle : MonoBehaviour
    {
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