namespace Balloondle.UI
{
    public class JoystickMoverLogic
    {
        public Touch ListenedTouch { get; private set; }

        public void ListenToTouch(Touch touch)
        {
            ListenedTouch = touch;
        }
    }
}