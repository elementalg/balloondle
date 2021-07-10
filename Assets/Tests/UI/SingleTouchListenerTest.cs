using Balloondle.UI;
using NUnit.Framework;

namespace Tests.UI
{
    public class SingleTouchListenerTest
    {
        [Test]
        public void IsNotListeningByDefault()
        {
            SingleTouchListener touchListener = new SingleTouchListener();
            
            Assert.False(touchListener.CurrentlyListening);
        }
    }
}