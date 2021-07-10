using UnityEngine.InputSystem.EnhancedTouch;

namespace Balloondle.UI
{
    /// <summary>
    /// Utility provider for listening only to a specific touch as long as it lasts.
    /// </summary>
    public class SingleTouchListener
    {
        private const int NoListenedTouch = -1;
        
        private int _listenedTouchListener = NoListenedTouch;
        
        public bool CurrentlyListening => _listenedTouchListener != NoListenedTouch;

        /// <summary>
        /// Start listening to the specified touch only if there's not a touch being listened to.
        /// </summary>
        /// <param name="touch">Touch whose phase is 'Began'.</param>
        public void OnTouchBegan(Touch touch)
        {
            if (CurrentlyListening)
            {
                return;
            }

            _listenedTouchListener = touch.touchId;
        }

        /// <summary>
        /// Checks whether or not the touch is the one being listened.
        /// </summary>
        /// <param name="touch">Touch checked if it is the one being listened.</param>
        /// <returns></returns>
        public bool IsTouchBeingListened(Touch touch)
        {
            return touch.touchId == _listenedTouchListener;
        }

        /// <summary>
        /// Stop listening to the touch, if it is the one we are listening to, since it does not exist anymore.
        /// </summary>
        /// <param name="touch">Touch checked if it is the one being listened.</param>
        public void OnTouchEnded(Touch touch)
        {
            if (!IsTouchBeingListened(touch))
            {
                return;
            }

            _listenedTouchListener = NoListenedTouch;
        }
    }
}