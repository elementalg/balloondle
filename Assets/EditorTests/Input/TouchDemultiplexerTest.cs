using Balloondle.Input;
using NUnit.Framework;
using UnityEngine.InputSystem;

namespace EditorTests.UI
{
    public class TouchDemultiplexerTest
    {
        private PointerDemultiplexer _touchDemultiplexer;

        [SetUp]
        public void Initialize()
        {
            _touchDemultiplexer = new PointerDemultiplexer();
        }
        
        [Test]
        public void TransfersTouchToFirstOutput()
        {
            bool outputCalled = false;
            Touch touchThatJustBegun = new Touch(1) {Phase = TouchPhase.Began};

            _touchDemultiplexer.AddOutputToQueue(touch =>
            {
                outputCalled = true;
            });
            _touchDemultiplexer.OnPointerUpdate(touchThatJustBegun);
            
            Assert.True(outputCalled, "Output has not been called.");
        }

        [Test]
        public void TransfersTouchOnlyToSelectedOutput()
        {
            // Random numbers used as id.
            Touch touchForFirstOutput = new Touch(1431) {Phase = TouchPhase.Began};
            Touch touchForSecondOutput = new Touch(5654) {Phase = TouchPhase.Began};
            bool firstOutputCorrect = false;
            bool secondOutputCorrect = false;
            
            _touchDemultiplexer.AddOutputToQueue(touch =>
            {
                firstOutputCorrect = (touch.PointerId == touchForFirstOutput.TouchId);
            });
            _touchDemultiplexer.AddOutputToQueue(touch =>
            {
                secondOutputCorrect = (touch.PointerId == touchForSecondOutput.TouchId);
            });
            _touchDemultiplexer.OnPointerUpdate(touchForFirstOutput);
            _touchDemultiplexer.OnPointerUpdate(touchForFirstOutput);
            _touchDemultiplexer.OnPointerUpdate(touchForSecondOutput);
            _touchDemultiplexer.OnPointerUpdate(touchForFirstOutput);
            
            Assert.True(firstOutputCorrect, "First output has received touch updates from " +
                                            "other touches not assigned to it.");
            Assert.True(secondOutputCorrect, "Second output has received touch updates from " +
                                             "other touches not assigned to it.");
        }

        [Test]
        public void DoesNotTransferTouchAfterTouchEnded()
        {
            const int expectedCallCount = 3;
            int callCount = 0;
            Touch touchWhichWillEnd = new Touch(32) {Phase = TouchPhase.Began};
            
            _touchDemultiplexer.AddOutputToQueue(touch =>
            {
                callCount++;
            });
            _touchDemultiplexer.OnPointerUpdate(touchWhichWillEnd);
            touchWhichWillEnd.Phase = TouchPhase.Moved;
            _touchDemultiplexer.OnPointerUpdate(touchWhichWillEnd);
            touchWhichWillEnd.Phase = TouchPhase.Ended;
            _touchDemultiplexer.OnPointerUpdate(touchWhichWillEnd);
            
            // This call must be ignored by the touch demultiplexer, if it is not ignored, and he output action is
            // called, then the demultiplexer does not work correctly.
            touchWhichWillEnd.Phase = TouchPhase.Began;
            _touchDemultiplexer.OnPointerUpdate(touchWhichWillEnd);

            Assert.AreEqual(expectedCallCount, callCount);
        }
    }
}