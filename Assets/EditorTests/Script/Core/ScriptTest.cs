using System;
using Balloondle.Script.Core;
using NUnit.Framework;

namespace EditorTests.Script.Core
{
    public class ScriptTest
    {
        private Balloondle.Script.Core.ScriptText _scriptText;

        [SetUp]
        public void Initialize()
        {
            _scriptText = new Balloondle.Script.Core.ScriptText();
        }

        [Test]
        public void ExceptionOnWriteNullEntry()
        {
            Assert.Throws<ArgumentException>(() => _scriptText.Write(null));
        }

        [Test]
        public void ExceptionOnWriteAfterRead()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _scriptText.ReadNext();
                _scriptText.Write(new SilenceEntry(0ul, 0f));
            });
        }

        [Test]
        public void ReturnsCorrectlyExpectedEntry()
        {
            SilenceEntry firstEntry = new SilenceEntry(0ul, 1);
            SilenceEntry secondEntry = new SilenceEntry(1ul, 2);
            
            _scriptText.Write(firstEntry);
            _scriptText.Write(secondEntry);
            
            Assert.True(ReferenceEquals(firstEntry, _scriptText.ReadNext()));
            Assert.True(ReferenceEquals(secondEntry, _scriptText.ReadNext()));
        }

        [Test]
        public void HasNextDetectsRemainingEntriesCorrectly()
        {
            Assert.False(_scriptText.HasNext());
            
            _scriptText.Write(new SilenceEntry(0ul, 1));
            
            Assert.True(_scriptText.HasNext());

            _scriptText.ReadNext();
            
            Assert.False(_scriptText.HasNext());
        }

        [Test]
        public void ExceptionOnReadEmptyScript()
        {
            Assert.Throws<InvalidOperationException>(() => _scriptText.ReadNext());
        }
    }
}