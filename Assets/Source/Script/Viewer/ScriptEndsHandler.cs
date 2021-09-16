using UnityEngine;

namespace Balloondle.Script.Viewer
{
    public abstract class ScriptEndsHandler : ScriptableObject
    {
        public abstract void OnScriptStart();
        public abstract void OnScriptEnd();
    }
}