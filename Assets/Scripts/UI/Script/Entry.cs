﻿namespace Balloondle.UI.Script
{
    public abstract class Entry
    {
        public float Duration { get; }

        protected Entry(float duration)
        {
            Duration = duration;
        }
    }
}