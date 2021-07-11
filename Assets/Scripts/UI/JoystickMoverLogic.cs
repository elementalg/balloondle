using JetBrains.Annotations;

namespace Balloondle.UI
{
    public class JoystickMoverLogic
    {
        [CanBeNull] 
        public Touch ListenedTouch { get; private set; }

        /// <summary>
        /// Starts listening to a touch, only if not already listening to an ongoing one, otherwise it gets ignored.
        /// </summary>
        /// <param name="touch"></param>
        /// <returns>True if started listening to the passed touch, false if it got ignored.</returns>
        public bool ListenToTouch(Touch touch)
        {
            if (ListenedTouch != null)
            {
                return false;
            }

            ListenedTouch = touch;
            return true;
        }
    }
}