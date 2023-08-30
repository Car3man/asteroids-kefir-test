using System.Numerics;

namespace Asteroids.Model
{
    public struct PhysicsBodyComponent
    {
        public Vector2 Acceleration;
        public Vector2 Velocity;
        public float Drag;
        public int CollisionLayer;
        public int CollisionMask;

        public PhysicsBodyComponent(float drag, int collisionLayer, int collisionMask) : this()
        {
            Drag = drag;
            CollisionLayer = collisionLayer;
            CollisionMask = collisionMask;
        }
    }
}