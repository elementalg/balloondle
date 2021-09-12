using Balloondle.Script.Core;
using UnityEngine;

namespace Editor.Script
{
    public class EditorEntryStyleComponent
    {
        public string StyleName = "";
        public EntryType EntryType = EntryType.Narrative;
        public GameObject Prefab = null;
    }
}