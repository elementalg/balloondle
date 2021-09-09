namespace Balloondle.UI.Script.Core
{
    public class CharacterEntry : Entry
    {
        public string Text { get; }
        public Character CharacterData { get; }
        
        public CharacterEntry(float duration, string text, Character character) : base(duration)
        {
            Text = text;
            CharacterData = character;
        }
    }
}