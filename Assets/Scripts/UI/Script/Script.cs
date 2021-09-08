using System;
using System.Collections.Generic;

namespace Balloondle.UI.Script
{
    public class Script
    {
        public enum State
        {
            WAITING_FOR_WRITE,
            WRITTEN,
            READ,
        }
        
        private readonly Queue<Entry> _entries;
        private State _state;
        
        public Script()
        {
            _entries = new Queue<Entry>();
            _state = State.WAITING_FOR_WRITE;
        }

        public void Write(Entry entry)
        {
            if (_state == State.READ)
            {
                throw new InvalidOperationException("Cannot write to the script if it has already been read.");
            }

            if (entry == null)
            {
                throw new ArgumentException("A non-null entry is required.");
            }

            _entries.Enqueue(entry);
        }

        public Entry ReadNext()
        {
            _state = State.READ;
            
            return _entries.Dequeue();
        }
    }
}