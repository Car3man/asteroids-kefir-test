using System.Numerics;

namespace Asteroids.Model
{
    public struct TransformComponent
    {
        public Vector2 Position;
        public float Angle;
        public float Scale;

        public TransformComponent(Vector2 position, float angle) : this()
        {
            Position = position;
            Angle = angle;
        }

        public TransformComponent(Vector2 position, float angle, float scale)
        {
            Position = position;
            Angle = angle;
            Scale = scale;
        }
    }
}