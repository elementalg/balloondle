using JetBrains.Annotations;

namespace Balloondle.UI
{
    public class JoystickMoverLogic
    {
        [CanBeNull] 
        public Touch ListenedTouch { get; private set; }

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