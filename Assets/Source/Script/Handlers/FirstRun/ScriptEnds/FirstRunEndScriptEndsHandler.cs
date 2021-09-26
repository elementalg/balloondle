using System;
using UnityEngine;

namespace Balloondle.Script.Handlers.FirstRun.ScriptEnds
{
    [CreateAssetMenu(fileName = "FirstRunEndScriptEndsHandler",
        menuName = "Script/Ends Handler/First Run End", order = 1)]
    public class FirstRunEndScriptEndsHandler : ScriptEndsHandler
    {
        [SerializeField] 
        private GameObject m_ScriptEasingInAndOutPrefab;

        [SerializeField] 
        private GameObject m_UIEventSystemPrefab;
        
        [SerializeField] 
        private GameObject m_FirstRunContinuePrefab;

        private Animator _animator;

        private void OnDestroy()
        {
            if (_animator != null)
            {
                Destroy(_animator.gameObject);
            }
        }

        public override void OnScriptStart()
        {
            InitializeAnimation();
            
            _animator.Play("BlurIn");
        }
        
        private void InitializeAnimation()
        {
            if (m_ScriptEasingInAndOutPrefab == null)
            {
                throw new InvalidOperationException("Prefab containing animation is required.");
            } 
            
            _animator = GameObject.Instantiate(m_ScriptEasingInAndOutPrefab).GetComponent<Animator>();
            
            if (_animator == null)
            {
                throw new InvalidOperationException("Animator is required.");
            }
        }

        public override void OnScriptEnd()
        {
            if (FindObjectOfType<Canvas>() == null)
            {
                throw new InvalidOperationException("Canvas is missing from scene.");
            }

            GameObject.Instantiate(m_UIEventSystemPrefab);
            
            Transform canvasTransform = FindObjectOfType<Canvas>().transform;
            GameObject.Instantiate(m_FirstRunContinuePrefab, canvasTransform);
            
            PlayerPrefs.SetInt(StartLevelLoader.IsFirstRunKey, 0);
        }
    }
}