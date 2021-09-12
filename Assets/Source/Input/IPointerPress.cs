using UnityEngine;

namespace Balloondle.Input
{
    public interface IPointerPress
    {
        public abstract Vector2 ScreenPosition { get; set; }
        public abstract Vector2 StartScreenPosition { get; set; }
        public abstract double StartTime { get; set; }
        public abstract int PointerId { get; set; }
        public abstract PointerPhase PointerPhase { get; set; }

        public bool HasEnded();
    }
}
