namespace Asteroids.Model
{
    public struct PhysicsCollisionComponent
    {
        public int EntityA;
        public int EntityB;

        public PhysicsCollisionComponent(int entityA, int entityB)
        {
            EntityA = entityA;
            EntityB = entityB;
        }
    }
}