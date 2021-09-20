namespace Balloondle.Script.Core
{
    public class NarrativeEntry : Entry
    {
        public string Text { get; }

        public NarrativeEntry(int id, float duration, string text) : base(id, duration)
        {
            Text = text;
        }
        
        public NarrativeEntry(int id, float duration, ExpireEvent expireEvent, string text) : 
            base(id, duration, expireEvent)
        {
            Text = text;
        }
    }
}