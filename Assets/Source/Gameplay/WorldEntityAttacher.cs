using UnityEngine;

namespace Balloondle.Gameplay
{
    public abstract class WorldEntityAttacher : ScriptableObject
    {
        public abstract void Attach(WorldEntity start, Vector3 startAnchor, WorldEntity end, Vector3 endAnchor);
        public abstract bool IsAttached(WorldEntity entity);
    }
}