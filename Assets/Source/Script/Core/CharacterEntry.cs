namespace Balloondle.Script.Core
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

        public CharacterEntry(float duration, ExpireEvent expireEvent, string text, Character character) : 
            base(duration, expireEvent)
        {
            Text = text;
            CharacterData = character;
        }
    }
}