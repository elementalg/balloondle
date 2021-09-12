using UnityEngine;

namespace Balloondle.Input
{
    public class        MouseClick : IPointerPress
    {
        public const int MouseLeftClickButtonPointerID = -1;
        public const int MouseMiddleClickButtonPointerID = -2;
        public const int MouseRightClickButtonPointerID = -3;

        public Vector2 screenPosition { get; set; }
        public Vector2 startScreenPosition { get; set; }
        public double startTime { get; set; }
        public int pointerId { get; set; }
        public PointerPhase pointerPhase { get; set; }

        public bool HasEnded()
        {
            return pointerPhase == PointerPhase.Ended || pointerPhase == PointerPhase.None;
        }
    }
}