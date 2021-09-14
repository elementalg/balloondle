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
        private float _entryElapsedTime;
        private IEntryDirector _currentEntryDirector;

        private bool _hasExpireEventBeenTriggered;

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

            InitializeScript();
        }

        private void InitializeScript()
        {
            _currentEntry = _scriptContainer.ReadNext();
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
                        m_ScriptStyle.m_Components[0], m_SceneCanvas);
                    
                    break;
            }

            return true;
        }
        
        private void DirectScript()
        {
            if (HasCurrentEntryExpired())
            {
                if (!_scriptContainer.HasNext())
                {
                    if (_currentEntry == null)
                    {
                        return;
                    }
                    
                    _currentEntryDirector.Out();
                    m_ScriptEndsHandler.OnScriptEnd();
                    
                    _currentEntry = null;
                    _currentEntryDirector = null;
                    _entryElapsedTime = 0f;
                    
                    Destroy(this, 1f);
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

            // keep waiting for the expire event to be triggered.
            if (_currentEntry.Expire.Enabled && !_hasExpireEventBeenTriggered)
            {
                return false;
            }

            float entryProgress = _entryElapsedTime / _currentEntry.Duration;

            return entryProgress >= 1f;
        }

        private void InitializeNextEntryDirector()
        {
            Entry nextEntry = _scriptContainer.ReadNext();
            _entryElapsedTime = 0f;
            _hasExpireEventBeenTriggered = false;

            IEntryDirector previousDirector = _currentEntryDirector;

            _currentEntry = nextEntry;

            if (UpdateEntryDirectorForCurrentEntry())
            {
                previousDirector.Out();
                _currentEntryDirector.In();
            }
        }

        private void Update()
        {
            _entryElapsedTime += Time.deltaTime;
            
            DirectScript();
        }

        public void TriggerExpireEvent(string value)
        {
            if (_currentEntry.Expire.Enabled && _currentEntry.Expire.Value.Equals(value))
            {
                _entryElapsedTime = 0f;
                _hasExpireEventBeenTriggered = true;
            }
        }
    }
}