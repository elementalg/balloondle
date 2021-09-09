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
        
        /// <summary>
        /// Checks whether or not there are entries left in the script.
        /// </summary>
        /// <returns>True if there are entries left in the script, false otherwise.</returns>
        public bool HasNext()
        {
            return _entries.Count > 0;
        }
        
        public Entry ReadNext()
        {
            if (_entries.Count == 0)
            {
                throw new InvalidOperationException("Cannot read script if it is empty.");
            }
        
            _state = State.READ;
            
            return _entries.Dequeue();
        }
    }
}