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
        /// <param name="duration">seconds of silence.</param>
        public SilenceEntry(float duration) : base(duration)
        {
            
        }
    }
}