namespace Asteroids.ECS
{
    public struct Component<T>
    {
        public bool Exists;
        public T Value;
    }
}