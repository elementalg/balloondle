using System;
using UnityEngine;

namespace Balloondle.Script.Viewer
{
    /// <summary>
    /// Declare standard in and out transitions for script's entries animations.
    /// </summary>
    public class InOutController : MonoBehaviour
    {
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

        public void InText()
        {
            _animator.Play("InText");
        }

        public void OutText()
        {
            _animator.Play("OutText");
        }
        
        public void Out()
        {
            _animator.Play("Out");
        }
    }
}
