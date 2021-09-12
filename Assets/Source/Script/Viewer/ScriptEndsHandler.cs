using UnityEngine;

namespace Balloondle.Script.Viewer
{
    public abstract class ScriptEndsHandler : MonoBehaviour
    {
        public abstract void OnScriptStart();
        public abstract void OnScriptEnd();
    }
}