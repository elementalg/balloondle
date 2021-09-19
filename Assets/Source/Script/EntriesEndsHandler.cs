using UnityEngine;

namespace Balloondle.Script
{
    public abstract class EntriesEndsHandler : ScriptableObject
    {
        public abstract void OnEntryStart(ulong entryId);
        public abstract void OnEntryEnd(ulong entryId);
    }
}