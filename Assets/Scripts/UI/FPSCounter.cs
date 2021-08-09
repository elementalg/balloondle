using System;
using UnityEngine;
using UnityEngine.UI;

namespace Balloondle.UI
{
    public class FPSCounter : MonoBehaviour
    {
        private Text _text;
        private float _framesRendered = 0f;
        private float _timeRunning = 0f;
        private float _minimumFPS = Single.PositiveInfinity;
        private float _maximumFPS = Single.NegativeInfinity;

        private void OnEnable()
        {
            Application.targetFrameRate = 60;
            
            if (GetComponent<Text>() == null)
            {
                throw new InvalidOperationException(
                    "FPSCounter requires GameObject to contain a 'Text' component.");
            }
            
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            OnFrameRender();

            float instantFPS = 1f / Time.deltaTime;

            if (instantFPS < _minimumFPS)
            {
                _minimumFPS = instantFPS;
            } 
            else if (instantFPS > _maximumFPS)
            {
                _maximumFPS = instantFPS;
            }
            
            _text.text =
                $"Average: {_framesRendered / _timeRunning:0.##}\nFPS: {instantFPS:0.##}\nMin: {_minimumFPS:0.##}\nMax: {_maximumFPS:0.##}";
        }

        private void OnFrameRender()
        {
            _framesRendered += 1f;
            _timeRunning += Time.deltaTime;
            
            if (float.IsPositiveInfinity(_framesRendered))
            {
                _framesRendered = 0f;
                _timeRunning = 0.00001f;
            }
        }
    }
}
