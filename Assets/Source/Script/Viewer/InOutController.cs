using System;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    /// <summary>
    /// Declare standard in and out transitions for script's entries animations.
    /// </summary>
    public class InOutController : MonoBehaviour
    {
        public Action m_InEnd;
        public Action m_InTextEnd;
        public Action m_OutTextEnd;
        public Action m_OutEnd;
        
        private Animator _animator;

        private void OnEnable()
        {
            if (GetComponent<Animator>() == null)
            {
                throw new InvalidOperationException("Missing animation component.");
            }

            _animator = GetComponent<Animator>();
        }

        public void In()
        {
            _animator.Play("In");
        }

        private void InEnd()
        {
            m_InEnd?.Invoke();
        }

        public void InText()
        {
            _animator.Play("InText");
        }

        private void InTextEnd()
        {
            m_InTextEnd?.Invoke();
        }

        public void OutText()
        {
            _animator.Play("OutText");
        }

        private void OutTextEnd()
        {
            m_OutTextEnd?.Invoke();
        }
        
        public void Out()
        {
            _animator.Play("Out");
        }

        private void OutEnd()
        {
            m_OutEnd?.Invoke();
        }
    }
}
