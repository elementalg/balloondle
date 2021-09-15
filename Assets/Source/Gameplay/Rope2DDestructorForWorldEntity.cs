using Balloondle.Gameplay.Physics2D;

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