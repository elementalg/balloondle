using System;
using UnityEngine;

namespace Balloondle.Script
{
    public class ScriptDirector : MonoBehaviour
    {
        [SerializeField, Tooltip("Script to begin on start.")] 
        private ScriptPreset m_StartWithScript;
        
        private ScriptPreset _scriptPreset;
        
        private bool _scriptEnabled;

        private void Start()
        {
            if (m_StartWithScript != null)
            {
                StartScript(m_StartWithScript);
            }
        }

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
            
            _scriptPreset = script;
            _scriptPreset.Start();
            _scriptPreset.OnScriptEnd += OnScriptEnd;
            
            _scriptEnabled = true;
        }

        public void OnScriptEnd()
        {
            _scriptPreset = null;
            _scriptEnabled = false;
        }
        
        private void Update()
        {
            if (_scriptEnabled)
            {
                _scriptPreset.Update();
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
                return _scriptPreset.TriggerExpireEvent(value);
            }

            return false;
        }
    }
}