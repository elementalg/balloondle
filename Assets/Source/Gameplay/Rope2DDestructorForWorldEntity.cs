using Balloondle.Gameplay.Physics2D;
using Balloondle.Gameplay.World;

namespace Balloondle.Gameplay
{
    public class Rope2DDestructorForWorldEntity : IRope2DCustomDestructor
    {
        private readonly WorldEntity _ropeWorldEntity;
        
        public Rope2DDestructorForWorldEntity(WorldEntity ropeWorldEntity)
        {
            _ropeWorldEntity = ropeWorldEntity;
        }
        
        public void OnBreakRope()
        {
            _ropeWorldEntity.Damage(_ropeWorldEntity.Health);
        }
    }
}