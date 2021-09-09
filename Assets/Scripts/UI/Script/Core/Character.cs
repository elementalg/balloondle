namespace Balloondle.UI.Script.Core
{
    public readonly struct Character
    {
        public ulong Id { get; }
        public string Name { get; }
        
        public Character(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}