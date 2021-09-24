using System;
using System.Collections.Generic;
using Balloondle.Script.Core;
using Balloondle.Script.Data;
using Balloondle.Script.Viewer;
using UnityEngine;

namespace Balloondle.Script
{
    [CreateAssetMenu(fileName = "ScriptPreset", menuName = "Script/Preset", order = 1)]
    public class ScriptPreset : ScriptableObject
    {
        public Action OnScriptEnd;

        [SerializeField] 
        private TextAsset m_ScriptJsonFile;

        [SerializeField] 
        private ScriptStyle m_ScriptStyle;

        [SerializeField] 
        private ScriptEndsHandler m_ScriptEndsHandler;

        [SerializeField] 
        private List<EntriesEndsHandler> m_EntriesEndsHandler;

        private Canvas _sceneCanvas;

        private ScriptText _scriptText;

        private Entry _currentEntry;
        private float _entryElapsedTime;
        private IEntryDirector _currentEntryDirector;

        private bool _hasExpireEventBeenTriggered;

        public void Start()
        {
            if (m_ScriptJsonFile == null)
            {
                throw new InvalidOperationException("A script must be assigned.");
            }

            if (m_ScriptStyle == null)
            {
                throw new InvalidOperationException("A script style must be assigned.");
            }

            _sceneCanvas = FindObjectOfType<Canvas>();

            if (_sceneCanvas == null)
            {
                throw new InvalidOperationException("Scene contains no Canvas.");
            }
            
            string serializedScript = m_ScriptJsonFile.text;
            ScriptExtractorFromJson scriptExtractor = new ScriptExtractorFromJson();
            _scriptText = scriptExtractor.FromJson(serializedScript);

            if (!_scriptText.HasNext())
            {
                throw new InvalidOperationException("Script must contain at least an entry.");
            }

            if (m_ScriptEndsHandler != null)
            {
                m_ScriptEndsHandler.OnScriptStart();
            }

            InitializeScript();
        }

        private void InitializeScript()
        {
            _currentEntry = _scriptText.ReadNext();
            _entryElapsedTime = 0f;

            if (UpdateEntryDirectorForCurrentEntry())
            {
                _currentEntryDirector.In();
            }
        }

        /// <summary>
        /// Creates, or updates, an entry director.
        /// </summary>
        /// <returns>True if the new entry must be eased In, false otherwise.</returns>
        /// <exception cref="NotImplementedException">if a NarrativeEntry is detected.</exception>
        private bool UpdateEntryDirectorForCurrentEntry()
        {
            _sceneCanvas = FindObjectOfType<Canvas>();

            if (_sceneCanvas == null)
            {
                throw new InvalidOperationException("Scene contains no Canvas.");
            }
            
            switch (_currentEntry)
            {
                case SilenceEntry _:
                    _currentEntryDirector = new SilenceEntryDirector();
                    break;
                case NarrativeEntry _:
                    throw new NotImplementedException("NarrativeEntry direction has not been implemented yet.");
                case CharacterEntry entry:
                    if (_currentEntryDirector != null &&
                        _currentEntryDirector is CharacterEntryDirector currentDirector)
                    {
                        if (entry.CharacterData.Id == currentDirector.CharacterId)
                        {
                            currentDirector.UpdateText(entry);
                            return false;
                        }
                    } 

                    _currentEntryDirector = new CharacterEntryDirector(entry,
                        m_ScriptStyle.m_Components[0], _sceneCanvas);
                    
                    break;
            }

            return true;
        }
        
        private void DirectScript()
        {
            if (HasCurrentEntryExpired())
            {
                if (!_scriptText.HasNext())
                {
                    if (_currentEntry == null)
                    {
                        return;
                    }
                    
                    _currentEntryDirector.Out();

                    foreach (EntriesEndsHandler entriesEndsHandler in m_EntriesEndsHandler)
                    {
                        entriesEndsHandler.OnEntryEnd(_currentEntry.Id);
                    }

                    OnScriptEnd?.Invoke();
                    if (m_ScriptEndsHandler != null)
                    {
                        m_ScriptEndsHandler.OnScriptEnd();
                    }

                    _currentEntry = null;
                    _currentEntryDirector = null;
                    _entryElapsedTime = 0f;
                    
                    return;
                }

                InitializeNextEntry();
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

            // keep waiting for the expire event to be triggered.
            if (_currentEntry.Expire.Enabled && !_hasExpireEventBeenTriggered)
            {
                return false;
            }

            float entryProgress = _entryElapsedTime / _currentEntry.Duration;

            return entryProgress >= 1f;
        }

        private void InitializeNextEntry()
        {
            Entry nextEntry = _scriptText.ReadNext();
            _entryElapsedTime = 0f;
            _hasExpireEventBeenTriggered = false;
            
            foreach (EntriesEndsHandler entriesEndsHandler in m_EntriesEndsHandler) 
            {
                if (_currentEntry != null)
                {
                    entriesEndsHandler.OnEntryEnd(_currentEntry.Id);
                }
                
                entriesEndsHandler.OnEntryStart(nextEntry.Id);
            }

            IEntryDirector previousDirector = _currentEntryDirector;

            _currentEntry = nextEntry;

            if (UpdateEntryDirectorForCurrentEntry())
            {
                previousDirector.Out();
                _currentEntryDirector.In();
            }
        }

        public void Update()
        {
            _entryElapsedTime += Time.deltaTime;
            
            DirectScript();
        }

        public bool TriggerExpireEvent(string value)
        {
            if (_currentEntry.Expire.Enabled && _currentEntry.Expire.Value.Equals(value))
            {
                _entryElapsedTime = 0f;
                _hasExpireEventBeenTriggered = true;

                return true;
            }

            return false;
        }
    }
}