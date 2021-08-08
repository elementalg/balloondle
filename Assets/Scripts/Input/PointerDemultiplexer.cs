using System;
using System.Collections.Generic;
using UnityEngine;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Balloondle.Input
{
    /// <summary>
    /// Acts as a 'demultiplexer' by selecting a pointer for a single purpose/action.
    ///
    /// By this way, pointer's pressing are not passed through to every component listening to the pointers, but rather
    /// only to a specific pressing reserved for a component.
    /// </summary>
    public class PointerDemultiplexer
    {
        private const double PressBeganPhaseMaxDurationInSeconds = 0.25f;
        
        private List<Tuple<Action<IPointerPress>, Func<IPointerPress, bool>>> _queue;
        private Dictionary<long, Action<IPointerPress>> _selectedPointers;
        
        public PointerDemultiplexer()
        {
            _queue = new List<Tuple<Action<IPointerPress>, Func<IPointerPress, bool>>>();
            _selectedPointers = new Dictionary<long, Action<IPointerPress>>();
        }

        /// <summary>
        /// Look for reserving a pointer's press, which will be transferred to the specified output action.
        /// </summary>
        /// <param name="outputAction">Action which will be called on all the pointers' update calls.</param>
        public void AddOutputToQueue(Action<IPointerPress> outputAction)
        {
            _queue
                .Add(new Tuple<Action<IPointerPress>, Func<IPointerPress, bool>>(outputAction, (pointer) => true));
        }

        /// <summary>
        /// Look for reserving a pointer's pressing which will be transferred to the specified output action,
        /// only if the touch condition has been met.
        /// </summary>
        /// <param name="outputAction">Action which will be called on all the touches' update calls.</param>
        /// <param name="pointerPressCondition">Condition which must be met by the touch in order to be assigned
        /// to the specified output action.</param>
        public void AddOutputToQueue(Action<IPointerPress> outputAction,
            Func<IPointerPress, bool> pointerPressCondition)
        {
            _queue.Add(
                new Tuple<Action<IPointerPress>, Func<IPointerPress, bool>>(outputAction, pointerPressCondition));
        }

        /// <summary>
        /// Called for each touch's instance situation on a single frame.
        /// </summary>
        /// <param name="pointer"></param>
        public void OnPointerUpdate(IPointerPress pointer)
        {
            if (_selectedPointers.ContainsKey(pointer.pointerId))
            {
                TransferPointerToSelectedOutput(pointer);
        
                if (!IsPointerPressStillAlive(pointer))
                {
                    RemovePointerFromSelectedPointers(pointer);
                }
            }
            else
            {
                // Ignore if touch phase does not indicate it still exists.
                if (!IsPointerPressStillAlive(pointer))
                {
                    return;
                }

                // Ignore touch since there is no one in the queue.
                if (_queue.Count == 0)
                {
                    return;
                }

                if (!HasPointerBegunRecently(pointer))
                {
                    return;
                }
                
                // If touch has not been selected, then proceed to ignore it and don't transfer it to anyone.
                if (!SelectOutputFromQueueForPointer(pointer))
                {
                    return;
                }
                
                TransferPointerToSelectedOutput(pointer);
            }
        }

        private void TransferPointerToSelectedOutput(IPointerPress pointer)
        {
            if (!_selectedPointers.ContainsKey(pointer.pointerId))
            {
                throw new InvalidOperationException("Cannot transfer touch to a non-existing output.");
            }

            _selectedPointers[pointer.pointerId].Invoke(pointer);
        }

        private static bool IsPointerPressStillAlive(IPointerPress pointer)
        {
            return pointer.pointerPhase == PointerPhase.Began ||
                   pointer.pointerPhase == PointerPhase.Moved ||
                   pointer.pointerPhase == PointerPhase.Stationary;
        }

        private static bool HasPointerBegunRecently(IPointerPress pointer)
        {
            if (pointer.pointerPhase == PointerPhase.Began)
            {
                return true;
            }

            return (Time.realtimeSinceStartupAsDouble - pointer.startTime) <= PressBeganPhaseMaxDurationInSeconds;
        }

        private bool SelectOutputFromQueueForPointer(IPointerPress pointer)
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

                if (touchCondition.Invoke(pointer))
                {
                    _selectedPointers.Add(pointer.pointerId, output);
                    
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

        private void RemovePointerFromSelectedPointers(IPointerPress pointer)
        {
            if (!_selectedPointers.ContainsKey(pointer.pointerId))
            {
                throw new InvalidOperationException("Unable to remove an unselected touch.");
            }

            _selectedPointers.Remove(pointer.pointerId);
        }

        /// <summary>
        /// Cancels all selected touches.
        /// </summary>
        public void DeselectSelectedPointers()
        {
            Touch touch = new Touch();
            
            foreach (long touchId in _selectedPointers.Keys)
            {
                touch.touchId = (int)touchId;
                touch.phase = TouchPhase.Canceled;

                TransferPointerToSelectedOutput(touch);
            }
            
            _selectedPointers.Clear();
        }
    }
}