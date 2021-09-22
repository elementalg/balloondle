namespace Balloondle.Script.Core
{
    public struct ExpireEvent
    {
        public bool Enabled;
        public string Value;

        public ExpireEvent(bool enabled = false, string value = "")
        {
            Enabled = enabled;
            Value = value;
        }
    }
}