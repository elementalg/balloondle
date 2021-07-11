using Balloondle.UI;
using NUnit.Framework;

namespace Tests.UI
{
    public class JoystickMoverLogicTest
    {
        private JoystickMoverLogic _joystickMoverLogic;
        
        [SetUp]
        public void InitializeJoystickMoverLogic()
        {
            _joystickMoverLogic = new JoystickMoverLogic();
        }

        [Test]
        public void AssignsTouchIfNoneIsAssigned()
        {
            Touch touch = new Touch(1); // '1' has no special meaning.

            Assert.True(_joystickMoverLogic.ListenToTouch(touch));
            
            Assert.AreEqual(touch.touchId, _joystickMoverLogic.ListenedTouch.touchId);
        }

        [Test]
        public void IgnoresTouchIfListeningAlreadyToAOnGoingTouch()
        {
            // The numbers used as ID have no special meaning.
            Touch listenedTouch = new Touch(1);
            Touch notListenedTouch = new Touch(2);
            
            Assert.True(_joystickMoverLogic.ListenToTouch(listenedTouch));
            Assert.False(_joystickMoverLogic.ListenToTouch(notListenedTouch));
            
            Assert.AreEqual(listenedTouch.touchId, _joystickMoverLogic.ListenedTouch.touchId);
        }
    }
}