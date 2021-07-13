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
        
        private List<Tuple<Action<Touch>, Func<Touch, bool>>> _queue;
        private Dictionary<long, Action<Touch>> _selectedTouches;

        // TODO: Add clean up method for removing touches which got invalidated without the Demultiplexer knowing.
        
        public TouchDemultiplexer()
        {
            _queue = new List<Tuple<Action<Touch>, Func<Touch, bool>>>();
            _selectedTouches = new Dictionary<long, Action<Touch>>();
        }

        /// <summary>
        /// Look for reserving a touch, which will be transferred to the specified output action.
        /// </summary>
        /// <param name="outputAction">Action which will be called on all the touches' update calls.</param>
        public void AddOutputToQueue(Action<Touch> outputAction)
        {
            _queue.Add(new Tuple<Action<Touch>, Func<Touch, bool>>(outputAction, (touch) => true));
        }

        /// <summary>
        /// Look for reserving a touch which will be transferred to the specified output action,
        /// only if the touch condition has been met.
        /// </summary>
        /// <param name="outputAction">Action which will be called on all the touches' update calls.</param>
        /// <param name="touchCondition">Condition which must be met by the touch in order to be assigned
        /// to the specified output action.</param>
        public void AddOutputToQueue(Action<Touch> outputAction, Func<Touch, bool> touchCondition)
        {
            _queue.Add(new Tuple<Action<Touch>, Func<Touch, bool>>(outputAction, touchCondition));
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
                
                // If touch has not been selected, then proceed to ignore it and don't transfer it to anyone.
                if (!SelectOutputFromQueueForTouch(touch))
                {
                    return;
                }
                
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

        private bool SelectOutputFromQueueForTouch(Touch touch)
        {
            // Ignore request if empty.
            if (_queue.Count == 0)
            {
                return false;
            }

            const int noSelectedOutput = -1;
            int selectedOutput = noSelectedOutput;
            for (int i = 0; i < _queue.Count; i++)
            {
                var (output, touchCondition) = _queue[i];

                if (touchCondition.Invoke(touch))
                {
                    _selectedTouches.Add(touch.touchId, output);
                    
                    selectedOutput = i;
                    break;
                }
            }

            if (selectedOutput == noSelectedOutput)
            {
                return false;
            }

            _queue.RemoveAt(selectedOutput);
            return true;
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