using UnityEngine;

namespace Balloondle.Input
{
    public class        MouseClick : IPointerPress
    {
        public const int MouseLeftClickButtonPointerID = -1;
        public const int MouseMiddleClickButtonPointerID = -2;
        public const int MouseRightClickButtonPointerID = -3;

        public Vector2 ScreenPosition { get; set; }
        public Vector2 StartScreenPosition { get; set; }
        public double StartTime { get; set; }
        public int PointerId { get; set; }
        public PointerPhase PointerPhase { get; set; }

        public bool HasEnded()
        {
            return PointerPhase == PointerPhase.Ended || PointerPhase == PointerPhase.None;
        }
    }
}