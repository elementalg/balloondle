namespace Balloondle.Script.Core
{
    public class NarrativeEntry : Entry
    {
        public string Text { get; }

        public NarrativeEntry(float duration, string text) : base(duration)
        {
            Text = text;
        }
    }
}