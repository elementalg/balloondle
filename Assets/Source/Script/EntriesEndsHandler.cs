using UnityEngine;

namespace Balloondle.Script
{
    public abstract class EntriesEndsHandler : ScriptableObject
    {
        public abstract void OnEntryStart(int entryId);
        public abstract void OnEntryEnd(int entryId);
    }
}