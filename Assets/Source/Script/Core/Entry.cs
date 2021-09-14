namespace Balloondle.Script.Core
{
    public abstract class Entry
    {
        public float Duration { get; }
        public ExpireEvent Expire { get; }

        protected Entry(float duration)
        {
            Duration = duration;
            Expire = new ExpireEvent();
        }

        protected Entry(float duration, ExpireEvent expireEvent)
        {
            Duration = duration;
            Expire = expireEvent;
        }
    }
}