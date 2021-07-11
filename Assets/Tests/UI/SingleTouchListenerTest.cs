﻿using Balloondle.UI;
using NUnit.Framework;

namespace Tests.UI
{
    public class SingleTouchListenerTest
    {
        private SingleTouchListener _touchListener;

        [SetUp]
        public void InitializeTouchListener()
        {
            _touchListener = new SingleTouchListener();
        }
        
        [Test]
        public void IsNotListeningByDefault()
        {
            Assert.False(_touchListener.CurrentlyListening);
        }

        [Test]
        public void IsListeningAfterTouchBegan()
        {
            Touch touch = new Touch(1);
            
            _touchListener.OnTouchBegan(touch);
            
            Assert.True(_touchListener.CurrentlyListening);
            Assert.True(_touchListener.IsTouchBeingListened(touch));
        }

        [Test]
        public void DifferentiatesBetweenListenedTouchAndNotListenedOneCorrectly()
        {
            Touch touchBeingListened = new Touch(1);
            Touch touchNotBeingListened = new Touch(2);
            
            _touchListener.OnTouchBegan(touchBeingListened);
            
            Assert.False(_touchListener.IsTouchBeingListened(touchNotBeingListened));
        }
    }
}