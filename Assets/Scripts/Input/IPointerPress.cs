using UnityEngine;

namespace Balloondle.Input
{
    public interface IPointerPress
    {
        public abstract Vector2 screenPosition { get; set; }
        public abstract Vector2 startScreenPosition { get; set; }
        public abstract double startTime { get; set; }
        public abstract int pointerId { get; set; }
        public abstract PointerPhase pointerPhase { get; set; }

        public bool HasEnded();
    }
}
