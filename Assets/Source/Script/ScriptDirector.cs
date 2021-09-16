using System;
using UnityEngine;

namespace Balloondle.Script
{
    public class ScriptDirector : MonoBehaviour
    {
        private ScriptPreset _script;
        private bool _scriptEnabled;
        
        /// <summary>
        /// Starts the specified script.
        /// </summary>
        /// <param name="script"></param>
        /// <exception cref="NotSupportedException">if there's already a script which
        /// has not been finished yet.</exception>
        public void StartScript(ScriptPreset script)
        {
            if (_scriptEnabled)
            {
                throw new NotSupportedException("Cannot start another script simultaneously.");
            }
            
            _script = script;
            _script.Start();
            _script.OnScriptEnd += OnScriptEnd;
            
            _scriptEnabled = true;
        }

        public void OnScriptEnd()
        {
            _script = null;
            _scriptEnabled = false;
        }
        
        private void Update()
        {
            if (_scriptEnabled)
            {
                _script.Update();
            }
        }
        
        /// <summary>
        /// Tries to trigger the expire event on the current script.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the event has been triggered successfully, false otherwise.</returns>
        public bool TryTriggerExpireEvent(string value)
        {
            if (_scriptEnabled)
            {
                return _script.TriggerExpireEvent(value);
            }

            return false;
        }
    }
}