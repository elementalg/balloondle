using System;
using System.Collections.Generic;

namespace Balloondle.Script.Core
{
    /// <summary>
    /// Abstraction of an acting script, where dialogues (<see cref="CharacterEntry"/>),
    /// narrations (<see cref="NarrativeEntry"/>) and silences (<see cref="SilenceEntry"/>),
    /// are described through <b>entries</b> (<see cref="Entry"/>).
    /// </summary>
    public class Script
    {
        private enum State
        {
            WaitingForWrite,
            Written,
            Read,
        }
        
        private readonly Queue<Entry> _entries;
        private State _state;
        
        /// <summary>
        /// Initialize an empty Script waiting to be written to.
        /// </summary>
        public Script()
        {
            _entries = new Queue<Entry>();
            _state = State.WaitingForWrite;
        }

        /// <summary>
        /// Writes an entry to the script following a FIFO logic.
        ///
        /// FIFO: First in, first out.
        /// </summary>
        /// <param name="entry">Entry being added at the end of the script.</param>
        /// <exception cref="InvalidOperationException">if the script has been read partially or completely.</exception>
        /// <exception cref="ArgumentException">if the entry is null.</exception>
        public void Write(Entry entry)
        {
            if (_state == State.Read)
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The next entry from the script.</returns>
        /// <exception cref="InvalidOperationException">if there are no entries left to read
        /// within the script.</exception>
        public Entry ReadNext()
        {
            if (_entries.Count == 0)
            {
                throw new InvalidOperationException("Cannot read script if it is empty.");
            }
        
            _state = State.Read;
            
            return _entries.Dequeue();
        }
    }
}