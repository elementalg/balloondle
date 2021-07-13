using System;
using System.Collections.Generic;
using UnityEngine;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Balloondle.UI
{
    /// <summary>
    /// Acts as a 'demultiplexer' by selecting a touch for a single purpose/action.
    ///
    /// By this way, touches are not passed through to every component listening to the touchscreen, but rather
    /// only to a specific touch reserved for a component.
    /// </summary>
    public class TouchDemultiplexer
    {
        private const double TouchBeganPhaseMaxDurationInSeconds = 0.25f;
        
        private Queue<Action<Touch>> _queue;
        private Dictionary<long, Action<Touch>> _selectedTouches;

        // TODO: Add clean up method for removing touches which got invalidated without the Demultiplexer knowing.
        
        public TouchDemultiplexer()
        {
            _queue = new Queue<Action<Touch>>();
            _selectedTouches = new Dictionary<long, Action<Touch>>();
        }

        /// <summary>
        /// Look for reserving a touch, which will be transferred to the specified output action.
        /// </summary>
        /// <param name="outputAction">Action which will be called on all the touches' update calls.</param>
        public void AddOutputToQueue(Action<Touch> outputAction)
        {
            _queue.Enqueue(outputAction);
        }

        /// <summary>
        /// Called for each touch's instance situation on a single frame.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchUpdate(Touch touch)
        {
            if (_selectedTouches.ContainsKey(touch.touchId))
            {
                TransferTouchToSelectedOutput(touch);

                if (!IsTouchPhaseStillAlive(touch))
                {
                    RemoveTouchFromSelectedTouches(touch);
                }
            }
            else
            {
                // Ignore if touch phase does not indicate it still exists.
                if (!IsTouchPhaseStillAlive(touch))
                {
                    return;
                }

                // Ignore touch since there is no one in the queue.
                if (_queue.Count == 0)
                {
                    return;
                }

                if (!HasTouchBeganRecently(touch))
                {
                    return;
                }
                
                SelectOutputFromQueueForTouch(touch);
                TransferTouchToSelectedOutput(touch);
            }
        }

        private void TransferTouchToSelectedOutput(Touch touch)
        {
            if (!_selectedTouches.ContainsKey(touch.touchId))
            {
                throw new InvalidOperationException("Cannot transfer touch to a non-existing output.");
            }

            _selectedTouches[touch.touchId].Invoke(touch);
        }

        private static bool IsTouchPhaseStillAlive(Touch touch)
        {
            return touch.phase == TouchPhase.Began || 
                   touch.phase == TouchPhase.Moved ||
                   touch.phase == TouchPhase.Stationary;
        }

        private bool HasTouchBeganRecently(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                return true;
            }

            return (Time.realtimeSinceStartupAsDouble - touch.startTime) <= TouchBeganPhaseMaxDurationInSeconds;
        }

        private void SelectOutputFromQueueForTouch(Touch touch)
        {
            // Ignore request if empty.
            if (_queue.Count == 0)
            {
                return;
            }
            
            Action<Touch> output = _queue.Dequeue();

            _selectedTouches.Add(touch.touchId, output);
        }

        private void RemoveTouchFromSelectedTouches(Touch touch)
        {
            if (!_selectedTouches.ContainsKey(touch.touchId))
            {
                throw new InvalidOperationException("Unable to remove an unselected touch.");
            }

            _selectedTouches.Remove(touch.touchId);
        }
    }
}