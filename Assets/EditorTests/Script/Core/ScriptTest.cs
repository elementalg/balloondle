using System;
using Balloondle.Script.Core;
using NUnit.Framework;

namespace EditorTests.Script.Core
{
    public class ScriptTest
    {
        private Balloondle.Script.Core.ScriptContainer m_ScriptContainer;

        [SetUp]
        public void Initialize()
        {
            m_ScriptContainer = new Balloondle.Script.Core.ScriptContainer();
        }

        [Test]
        public void ExceptionOnWriteNullEntry()
        {
            Assert.Throws<ArgumentException>(() => m_ScriptContainer.Write(null));
        }

        [Test]
        public void ExceptionOnWriteAfterRead()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                m_ScriptContainer.ReadNext();
                m_ScriptContainer.Write(new SilenceEntry(0f));
            });
        }

        [Test]
        public void ReturnsCorrectlyExpectedEntry()
        {
            SilenceEntry firstEntry = new SilenceEntry(1);
            SilenceEntry secondEntry = new SilenceEntry(2);
            
            m_ScriptContainer.Write(firstEntry);
            m_ScriptContainer.Write(secondEntry);
            
            Assert.True(ReferenceEquals(firstEntry, m_ScriptContainer.ReadNext()));
            Assert.True(ReferenceEquals(secondEntry, m_ScriptContainer.ReadNext()));
        }

        [Test]
        public void HasNextDetectsRemainingEntriesCorrectly()
        {
            Assert.False(m_ScriptContainer.HasNext());
            
            m_ScriptContainer.Write(new SilenceEntry(1));
            
            Assert.True(m_ScriptContainer.HasNext());

            m_ScriptContainer.ReadNext();
            
            Assert.False(m_ScriptContainer.HasNext());
        }

        [Test]
        public void ExceptionOnReadEmptyScript()
        {
            Assert.Throws<InvalidOperationException>(() => m_ScriptContainer.ReadNext());
        }
    }
}