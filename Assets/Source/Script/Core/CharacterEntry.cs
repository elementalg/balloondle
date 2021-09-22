namespace Balloondle.Script.Core
{
    public class CharacterEntry : Entry
    {
        public string Text { get; }
        public Character CharacterData { get; }
        
        public CharacterEntry(int id, float duration, string text, Character character) : base(id, duration)
        {
            Text = text;
            CharacterData = character;
        }

        public CharacterEntry(int id, float duration, ExpireEvent expireEvent, string text, Character character) : 
            base(id, duration, expireEvent)
        {
            Text = text;
            CharacterData = character;
        }
    }
}