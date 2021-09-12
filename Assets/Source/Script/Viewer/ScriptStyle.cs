using System.Collections.Generic;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    public class ScriptStyle : MonoBehaviour
    {
        public List<EntryStyleComponent> _components;

        private void Start()
        {
            if (_components == null)
            {
                _components = new List<EntryStyleComponent>();
            }
        }

        public void DebugComponents()
        {

        }
    }
}