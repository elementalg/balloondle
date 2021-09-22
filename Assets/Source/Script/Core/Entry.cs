namespace Balloondle.Script.Core
{
    public abstract class Entry
    {
        public int Id { get; }
        public float Duration { get; }
        public ExpireEvent Expire { get; }

        protected Entry(int id, float duration)
        {
            Id = id;
            Duration = duration;
            Expire = new ExpireEvent();
        }

        protected Entry(int id, float duration, ExpireEvent expireEvent)
        {
            Id = id;
            Duration = duration;
            Expire = expireEvent;
        }
    }
}