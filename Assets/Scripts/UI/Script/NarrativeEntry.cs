namespace Balloondle.UI.Script
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