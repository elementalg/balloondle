using UnityEngine;

namespace Balloondle.Script
{
    public abstract class ScriptEndsHandler : ScriptableObject
    {
        public abstract void OnScriptStart();
        public abstract void OnScriptEnd();
    }
}