namespace Balloondle.Gameplay.Physics2D
{
    public interface IRope2DCustomDestructor
    {
        /// <summary>
        /// Method called instead of 'Destroy(gameObject)' on <see cref="Rope2D"/>'s 'Break' method.
        /// </summary>
        public void OnBreakRope();
    }
}