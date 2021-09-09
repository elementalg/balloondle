using System;
using Balloondle.UI.Script;
using NUnit.Framework;

namespace EditorTests.UI.Script
{
    public class ScriptTest
    {
        private Balloondle.UI.Script.Script _script;

        [SetUp]
        public void Initialize()
        {
            _script = new Balloondle.UI.Script.Script();
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
        public void ExceptionOnReadEmptyScript()
        {
            Assert.Throws<InvalidOperationException>(() => _script.ReadNext());
        }
    }
}