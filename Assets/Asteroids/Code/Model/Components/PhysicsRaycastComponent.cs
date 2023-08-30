namespace Asteroids.Model
{
    public struct PhysicsRaycastComponent
    {
        public float Length;
        public int CollisionMask;

        public PhysicsRaycastComponent(float length, int collisionMask)
        {
            Length = length;
            CollisionMask = collisionMask;
        }
    }
}