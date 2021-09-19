using System;
using Balloondle.Gameplay;
using UnityEngine;

namespace Balloondle.Script.Handlers.ScriptEnds
{
    [CreateAssetMenu(fileName = "FirstRunScriptEndsHandler", menuName = "Script/Ends Handler/First Run", order = 1)]
    public class FirstRunScriptEndsHandler : ScriptEndsHandler
    {
        [SerializeField] 
        private GameObject m_ScriptEasingInAndOutPrefab;

        [SerializeField] 
        private ScriptPreset m_WaitForMovementPreset;

        [SerializeField, Tooltip("Amount of time to wait before destroying the created Animation's GameObject.")]
        private float m_DestroyAnimationAfterTime = 1f;

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
            _animator.Play("BlurOut");
            
            Destroy(_animator.gameObject, m_DestroyAnimationAfterTime);
            
            WorldEntitySpawner worldEntitySpawner = FindObjectOfType<WorldEntitySpawner>();
            worldEntitySpawner.Spawn("Balloon", new Vector3(16.5f, -11.66f, 0f), Quaternion.identity);

            ScriptDirector director = FindObjectOfType<ScriptDirector>();
            director.StartScript(m_WaitForMovementPreset);
        }
    }
}