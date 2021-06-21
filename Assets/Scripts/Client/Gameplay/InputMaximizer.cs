using UnityEngine;

namespace Balloondle.Client
{
    public class InputMaximizer
    {
        private bool sampling;
        private float sampleStartTimestap;
        private float inputSampleDuration;
        private Vector2 sample;

        public InputMaximizer(float inputSampleDuration)
        {
            sampling = false;
            this.inputSampleDuration = inputSampleDuration;
            sample = new Vector2();
        }

        public void OnInput(Vector2 input)
        {
            if (!sampling)
            {
                sampling = true;
                sampleStartTimestap = Time.realtimeSinceStartup;
            }

            float elapsed = Time.realtimeSinceStartup - sampleStartTimestap;

            if (elapsed > inputSampleDuration)
            {
                RestartSample();
            }

            // Proceed to maximize input.
            if (Mathf.Abs(input.x) > Mathf.Abs(sample.x))
            {
                sample.x = input.x;
            }

            if (Mathf.Abs(input.y) > Mathf.Abs(sample.y))
            {
                sample.y = input.y;
            }
        }

        public Vector2? OnUpdate()
        {
            float elapsed = Time.realtimeSinceStartup - sampleStartTimestap;

            if (elapsed > inputSampleDuration)
            {
                if (sample.x != 0f && sample.y != 0f)
                {
                    Vector2 copy = new Vector2(sample.x, sample.y);
                    RestartSample();
                    return copy;
                }
                else
                {
                    RestartSample();
                }
            }

            return null;
        }

        private void RestartSample()
        {
            sample.x = 0f;
            sample.y = 0f;
            sampleStartTimestap = Time.realtimeSinceStartup;
        }
    }
}