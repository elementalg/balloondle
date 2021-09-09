using System;
using Balloondle.Script.Core;
using NUnit.Framework;

namespace EditorTests.Script.Core
{
    public class ScriptTest
    {
        private Balloondle.Script.Core.Script _script;

        [SetUp]
        public void Initialize()
        {
            _script = new Balloondle.Script.Core.Script();
        }

        [Test]
        public void ExceptionOnWriteNullEntry()
        {
            Assert.Throws<ArgumentException>(() => _script.Write(null));
        }

        [Test]
        public void ExceptionOnWriteAfterRead()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _script.ReadNext();
                _script.Write(new SilenceEntry(0f));
            });
        }

        [Test]
        public void ReturnsCorrectlyExpectedEntry()
        {
            SilenceEntry firstEntry = new SilenceEntry(1);
            SilenceEntry secondEntry = new SilenceEntry(2);
            
            _script.Write(firstEntry);
            _script.Write(secondEntry);
            
            Assert.True(ReferenceEquals(firstEntry, _script.ReadNext()));
            Assert.True(ReferenceEquals(secondEntry, _script.ReadNext()));
        }

        [Test]
        public void HasNextDetectsRemainingEntriesCorrectly()
        {
            Assert.False(_script.HasNext());
            
            _script.Write(new SilenceEntry(1));
            
            Assert.True(_script.HasNext());

            _script.ReadNext();
            
            Assert.False(_script.HasNext());
        }

        [Test]
        public void ExceptionOnReadEmptyScript()
        {
            Assert.Throws<InvalidOperationException>(() => _script.ReadNext());
        }
    }
}