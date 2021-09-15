using System;
using UnityEngine;

namespace Balloondle.Script.Viewer.Styles
{
    public class FirstRunScriptEndsHandler : ScriptEndsHandler
    {
        [SerializeField] 
        private GameObject m_ScriptEndAnimationPrefab;

        private Animator _animator;
        
        private void OnEnable()
        {
            if (m_ScriptEndAnimationPrefab == null)
            {
                throw new InvalidOperationException("Prefab containing animation is required.");
            } 
            
            _animator = Instantiate(m_ScriptEndAnimationPrefab).GetComponent<Animator>();
            
            if (_animator == null)
            {
                throw new InvalidOperationException("Animator is required.");
            }
        }

        private void OnDestroy()
        {
            if (_animator != null)
            {
                Destroy(_animator.gameObject);
            }
        }

        public override void OnScriptStart()
        {
            _animator.Play("BlurIn");
        }

        public override void OnScriptEnd()
        {
            _animator.Play("BlurOut");
            
            // TODO: Spawn balloon and move camera towards balloon before finally attaching the camera to the balloon.
        }
    }
}