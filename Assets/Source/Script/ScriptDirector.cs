using System;
using Balloondle.Script.Core;
using Balloondle.Script.Data;
using Balloondle.Script.Viewer;
using UnityEngine;

namespace Balloondle.Script
{
    public class ScriptDirector : MonoBehaviour
    {
        [SerializeField] 
        private TextAsset m_ScriptJsonFile;

        [SerializeField] 
        private ScriptStyle m_ScriptStyle;

        [SerializeField] 
        private ScriptEndsHandler m_ScriptEndsHandler;

        private void OnEnable()
        {
            if (m_ScriptJsonFile == null)
            {
                throw new InvalidOperationException("A script must be assigned.");
            }

            if (m_ScriptStyle == null)
            {
                throw new InvalidOperationException("A script style must be assigned.");
            }
            
            string serializedScript = m_ScriptJsonFile.text;
            ScriptExtractorFromJson scriptExtractor = new ScriptExtractorFromJson();
            ScriptContainer script = scriptExtractor.FromJson(serializedScript);

            if (!script.HasNext())
            {
                throw new InvalidOperationException("Script must contain at least an entry.");
            }
            
        }
    }
}