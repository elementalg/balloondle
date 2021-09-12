using System;
using Balloondle.Script.Core;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    [Serializable]
    public class EntryStyleComponent
    {
        public string Name;
        public EntryType EntryType;
        public GameObject Prefab;

        public EntryStyleComponent()
        {
            Name = "";
            EntryType = EntryType.Silence;
            Prefab = null;
        }
        
        public EntryStyleComponent(string name, EntryType entryType, GameObject prefab)
        {
            Name = name;
            EntryType = entryType;
            Prefab = prefab;
        }
    }
}