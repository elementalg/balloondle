using UnityEngine;

namespace Balloondle.Gameplay.World
{
    public abstract class WorldEntityAttacher : ScriptableObject
    {
        public abstract WorldEntity Attach(WorldEntity start, Vector3 startAnchor, WorldEntity end, Vector3 endAnchor);

        public abstract WorldEntity Attach(WorldEntity start, Vector3 startAnchor, WorldEntity end, Vector3 endAnchor,
            float distance);
        public abstract void Detach(WorldEntity attacher);
    }
}