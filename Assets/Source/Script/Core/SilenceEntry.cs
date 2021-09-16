namespace Balloondle.Script.Core
{
    /// <summary>
    /// Indicates a silence within the script.
    /// </summary>
    public class SilenceEntry : Entry
    {
        /// <summary>
        /// Creates a silence within the script with a duration of the specified value.
        /// </summary>
        /// <param name="id">id of the silence.</param>
        /// <param name="duration">seconds of silence.</param>
        public SilenceEntry(ulong id, float duration) : base(id, duration)
        {
            
        }
        
        public SilenceEntry(ulong id, float duration, ExpireEvent expireEvent) : base(id, duration, expireEvent)
        {
            
        }
    }
}