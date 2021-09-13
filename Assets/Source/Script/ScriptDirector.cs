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

        [SerializeField] 
        private Canvas m_SceneCanvas;

        private ScriptContainer _scriptContainer;

        private Entry _currentEntry;
        private float _entryStartTime;
        private IEntryDirector _currentEntryDirector;

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

            if (m_SceneCanvas == null)
            {
                Debug.LogWarning("Scene canvas has not been assigned. Using first encountered one.");
                m_SceneCanvas = FindObjectOfType<Canvas>();

                if (m_SceneCanvas == null)
                {
                    throw new InvalidOperationException("Scene contains no Canvas.");
                }
            }
            
            string serializedScript = m_ScriptJsonFile.text;
            ScriptExtractorFromJson scriptExtractor = new ScriptExtractorFromJson();
            _scriptContainer = scriptExtractor.FromJson(serializedScript);

            if (!_scriptContainer.HasNext())
            {
                throw new InvalidOperationException("Script must contain at least an entry.");
            }
        }

        private void Start()
        {
            m_ScriptEndsHandler.OnScriptStart();

            DirectScript();
        }

        private void DirectScript()
        {
            if (HasCurrentEntryExpired())
            {
                if (_currentEntry != null)
                {
                    _currentEntryDirector.Out();
                }
                
                if (!_scriptContainer.HasNext())
                {
                    m_ScriptEndsHandler.OnScriptEnd();
                    
                    _currentEntry = null;
                    _currentEntryDirector = null;
                    _entryStartTime = 0f;
                    
                    Destroy(this);
                    return;
                }

                InitializeNextEntryDirector();
            }
            else
            {
                _currentEntryDirector.Update();
            }
        }

        private bool HasCurrentEntryExpired()
        {
            if (_currentEntry == null)
            {
                return true;
            }

            float entryProgress = (Time.realtimeSinceStartup - _entryStartTime) / _currentEntry.Duration;

            return entryProgress >= 1f;
        }

        private void InitializeNextEntryDirector()
        {
            Entry nextEntry = _scriptContainer.ReadNext();
            _entryStartTime = Time.realtimeSinceStartup;

            switch (nextEntry)
            {
                case SilenceEntry _:
                    _currentEntryDirector = new SilenceEntryDirector();
                    break;
                case NarrativeEntry _:
                    throw new NotImplementedException("NarrativeEntry direction has not been implemented yet.");
                case CharacterEntry entry:
                    if (_currentEntry is CharacterEntry currentCharacter &&
                        entry.CharacterData.Id == currentCharacter.CharacterData.Id)
                    {
                        ((CharacterEntryDirector)_currentEntryDirector).UpdateText(currentCharacter);
                    }
                    else
                    {
                        _currentEntryDirector = new CharacterEntryDirector(entry,
                            m_ScriptStyle.m_Components[0], m_SceneCanvas);
                        _currentEntryDirector.In();
                    }
                    
                    break;
            }

            _currentEntry = nextEntry;
        }

        private void Update()
        {
            DirectScript();
        }
    }
}