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

            _joystickMoverLogic.ListenToTouch(touch);
            
            Assert.AreEqual(touch.touchId, _joystickMoverLogic.ListenedTouch.touchId);
        }
    }
}